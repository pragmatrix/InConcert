using System.IO;
using System.Threading.Tasks;

namespace InConcert
{
	static class FileSynchronizer
	{
		public static async Task syncFile(PathChange change, FileInfo sourceInfo, FileInfo targetInfo)
		{
			var compareResult = await compareFile(change.Source, change.Target);

			if (compareResult != CompareResult.Differ)
				return;

			// decide in which direction to sync.

			var changeLocation = change.ChangeLocation;
			if (changeLocation == ChangeLocation.Unknown)
			{
				changeLocation = sourceInfo.LastWriteTimeUtc >= targetInfo.LastWriteTimeUtc
					? ChangeLocation.AtSource
					: ChangeLocation.AtTarget;
			}

			switch (changeLocation)
			{
				case ChangeLocation.AtSource:
					await overwriteFile(change, change.Source, change.Target);
					break;

				case ChangeLocation.AtTarget:
					await overwriteFile(change, change.Target, change.Source);
					break;
			}
		}

		enum CompareResult
		{
			Unknown,
			Equal,
			Differ
		}

		static async Task<CompareResult> compareFile(string source, string target)
		{
			var sourceInfo = new FileInfo(source);
			var targetInfo = new FileInfo(target);

			var sKind = sourceInfo.getKindOf();
			var tKind = targetInfo.getKindOf();
			if (sKind != PathObjectKind.File || tKind != PathObjectKind.File)
				return CompareResult.Unknown;

			if (sourceInfo.Length != targetInfo.Length)
				return CompareResult.Differ;

			return await compareFilesWithSameSize(source, target);
		}

		static async Task<CompareResult> compareFilesWithSameSize(string source, string target)
		{
			const int BufSize = 0x10000;
			var sBuf = new byte[BufSize];
			var tBuf = new byte[BufSize];

			var sFile = File.Open(source, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite);
			var tFile = File.Open(target, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite);

			for(;;)
			{
				var t1 = sFile.ReadAsync(sBuf, 0, sBuf.Length);
				var t2 = tFile.ReadAsync(tBuf, 0, tBuf.Length);

				var readBytes = await Task.WhenAll(t1, t2);
				var read = readBytes[0];
				if (read != readBytes[1])
					return CompareResult.Differ;

				if (read == 0)
					return CompareResult.Equal;

				for (int i = 0; i != read; ++i)
					if (sBuf[i] != tBuf[i])
						return CompareResult.Differ;
			}
		}

		// we probably should copy the files by hand to ensure that the proper sharing flags are set.

		public static async Task copyFile(PathChange change, string source, string target)
		{
			change.log("copying file");

			if (!change.Configuration.Sync)
				return;

			await Task.Run(() => File.Copy(source, target, overwrite: false));
		}

		static async Task overwriteFile(PathChange change, string source, string target)
		{
			change.log("overwriting file");

			if (!change.Configuration.Sync)
				return;

			await Task.Run(() => File.Copy(source, target, overwrite: true));
		}
	}
}
