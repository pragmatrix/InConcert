using System;
using System.Threading;
using System.Threading.Tasks;
using Toolbox;

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

			if (configuration.Watch)
				beginWatching(sourcePath, targetPath);

			fullSynchronization(configuration)
				.Wait();

			if (configuration.Watch)
				Thread.Sleep(Timeout.Infinite);
		}

		static async Task fullSynchronization(Configuration Configuration)
		{

			


		}

		static void beginWatching(string sourcePath, string targetPath)
		{
			new Watcher(sourcePath, sourceChange);
			new Watcher(targetPath, targetChange);
		}

		static void sourceChange(string relativePath)
		{
		}

		static void targetChange(string relativePath)
		{
		}
	}
}
