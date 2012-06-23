using System;

namespace InConcert
{
	sealed class Configuration
	{
		public readonly string SourcePath;
		public readonly string TargetPath;

		public readonly bool Watch;
		public readonly bool Sync;
		public readonly bool Git;

		public Configuration(string sourcePath, string targetPath, bool watch, bool sync, bool git)
		{
			SourcePath = sourcePath;
			TargetPath = targetPath;
			Watch = watch;
			Sync = sync;
			Git = git;
		}

		public static Configuration fromCommandLine(params string[] args)
		{
			string sourcePath_ = null;
			string targetPath_ = null;
			bool watch = false;
			bool sync = false;
			bool git = false;

			foreach (var arg in args)
			{
				if (arg.StartsWith("--"))
				{
					switch (arg)
					{
						case "--watch": watch = true; break;
						case "--sync": sync = true; break;
						case "--git": git = true; break;

						default:
							throw new Exception(InvalidArgs);
					}
					continue;
				}

				if (sourcePath_ == null)
				{
					sourcePath_ = arg;
					continue;
				}

				if (targetPath_ == null)
				{
					targetPath_ = arg;
					continue;
				}

				throw new Exception(InvalidArgs);
			}

			if (sourcePath_ == null || targetPath_ == null)
				throw new Exception(InvalidArgs);

			return new Configuration(sourcePath_, targetPath_, watch, sync, git);
		}

		const string InvalidArgs =
			"InConcert.exe SourcePath TargetPath [--sync] [--watch] [--git]\n" +
					"  --sync   synchronize folders and files\n" +
					"  --watch  watch for changes" +
					"  --git    ignore .git and .gitignore";
	}
}
