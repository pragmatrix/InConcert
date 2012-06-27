using System;
using System.IO;
using System.Threading.Tasks;
using InConcert.Abstract;

namespace InConcert
{
	static class Synchronizer
	{
		public static async Task sync(PathChange change)
		{
			var sourceInfo = change.ReadFileSystem.query(change.Source);
			var targetInfo = change.ReadFileSystem.query(change.Target);

			var sourceKind = sourceInfo.Type;
			var targetKind = targetInfo.Type;
			if (sourceKind == PathType.NotExisting && targetKind == PathType.NotExisting)
			{
				change.log("not existing");
				return;
			}

			if (sourceKind == PathType.Ignored || targetKind == PathType.Ignored)
			{
				change.log("ignored");
				return;
			}

			if (targetKind == PathType.NotExisting)
			{
				await copyToTarget(change, sourceKind);
				return;
			}
		
			if (sourceKind == PathType.NotExisting)
			{
				await copyToSource(change, targetKind);
				return;
			}

			if (sourceKind != targetKind)
			{
				change.log("directory <-> file sync not implemented");
				return;
			}

			if (sourceKind == PathType.Directory)
				await DirectorySynchronizer.syncDirectory(change);
			else
				await FileSynchronizer.syncFile(change, sourceInfo, targetInfo);
		}

		static async Task copyToTarget(PathChange change, PathType kind)
		{
			switch (kind)
			{
				case PathType.Directory:
					DirectorySynchronizer.createDirectory(change, change.Target);
					break;

				case PathType.File:
					await FileSynchronizer.copyFile(change, change.Source, change.Target);
					break;
			}
		}

		static async Task copyToSource(PathChange change, PathType kind)
		{
			switch (kind)
			{
				case PathType.Directory:
					DirectorySynchronizer.createDirectory(change, change.Source);
					break;

				case PathType.File:
					await FileSynchronizer.copyFile(change, change.Target, change.Source);
					break;
			}
		}
	}
}
