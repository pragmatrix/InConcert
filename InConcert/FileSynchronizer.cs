using System.Threading.Tasks;
using InConcert.Abstract;

namespace InConcert
{
	static class FileSynchronizer
	{
		public static async Task syncFile(PathChange change, IPathInfo sourceInfo, IPathInfo targetInfo)
		{
			var compareResult = await compareFile(change.ReadFileSystem, change.Source, change.Target);

			if (compareResult != CompareResult.Differ)
				return;

			// decide in which direction to sync.

			var changeLocation = change.Location;
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

		static async Task<CompareResult> compareFile(IReadFileSystem rfs, string source, string target)
		{
			var sourceInfo = rfs.query(source);
			var targetInfo = rfs.query(target);

			var sType = sourceInfo.Type;
			var tType = targetInfo.Type;
			if (sType != PathType.File || tType != PathType.File)
				return CompareResult.Unknown;

			if (sourceInfo.Length != targetInfo.Length)
				return CompareResult.Differ;

			return await compareFilesWithSameSize(rfs, source, target);
		}

		static async Task<CompareResult> compareFilesWithSameSize(IReadFileSystem rfs, string source, string target)
		{
			const int BufSize = 0x10000;
			var sBuf = new byte[BufSize];
			var tBuf = new byte[BufSize];

			using (var sFile = rfs.open(source))
			using (var tFile = rfs.open(target))
			{

				for (; ; )
				{
					var t1 = sFile.readAsync(sBuf, 0, sBuf.Length);
					var t2 = tFile.readAsync(tBuf, 0, tBuf.Length);

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
		}

		// we probably should copy the files by hand to ensure that the proper sharing flags are set.

		public static async Task copyFile(PathChange change, string source, string target)
		{
			change.log("copying");

			await change.WriteFileSystem.copyAsync(source, target);
		}

		public static async Task overwriteFile(PathChange change, string source, string target)
		{
			change.log("overwriting");

			await change.WriteFileSystem.overwriteAsync(source, target);
		}
	}
}
