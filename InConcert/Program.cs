using System;
using System.Threading;
using System.Threading.Tasks;
using InConcert.Abstract;
using InConcert.System.IO;
using InConcert.Toolbox;

namespace InConcert
{
	class Program
	{
		public static int Main(string[] args)
		{
			try
			{
				internalMain(args);
				return 0;
			}
			catch (Exception e)
			{
				Console.Error.Write(e.Message);
				return 5;
			}
		}

		static void internalMain(string[] args)
		{
			var configuration = Configuration.fromCommandLine(args);

			var sourcePath = PathHelper.importSyncPath(configuration.SourcePath);
			var targetPath = PathHelper.importSyncPath(configuration.TargetPath);

			var readFileSystem = new ReadFileSystem();
			var writeFileSystem = configuration.Sync 
				? (IWriteFileSystem) new WriteFileSystem() 
				: new FakeWriteFileSystem();

			if (configuration.Watch)
			{
				Action<string, ChangeLocation> watcherChange = (path, loc) =>
					{
						var pc = new PathChange(configuration,
							ChangeMode.Shallow,
							loc,
							path,
							readFileSystem,
							writeFileSystem);

						Synchronizer.sync(pc);
					};

				beginWatching(
					readFileSystem,
					sourcePath,
					targetPath,
					path => watcherChange(path, ChangeLocation.AtSource),
					path => watcherChange(path, ChangeLocation.AtTarget));
			}

			deepSynchronization(configuration, readFileSystem, writeFileSystem)
				.Wait();

			if (configuration.Watch)
				Thread.Sleep(Timeout.Infinite);
		}

		static async Task deepSynchronization(Configuration Configuration, IReadFileSystem reader, IWriteFileSystem writer)
		{
			var pc = new PathChange(Configuration, ChangeMode.Deep, ChangeLocation.Unknown, "", reader, writer);
			await Synchronizer.sync(pc);
		}

		static IDisposable beginWatching(
			IReadFileSystem rfs, 
			string sourcePath, 
			string targetPath,
			Action<string> sourceChange, 
			Action<string> targetChange)
		{
			var src = rfs.beginWatch(sourcePath, sourceChange);
			var target = rfs.beginWatch(targetPath, targetChange);

			return new DisposeAction(() =>
				{
					target.Dispose();
					src.Dispose();
				});
		}
	}
}
