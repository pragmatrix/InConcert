using System;
using System.IO;
using System.Threading.Tasks;

namespace InConcert
{
	static class Synchronizer
	{
		public static async Task sync(PathChange change)
		{
			var sourceInfo = new FileInfo(change.Source);
			var targetInfo = new FileInfo(change.Target);

			var sourceKind = sourceInfo.getKindOf();
			var targetKind = targetInfo.getKindOf();
			if (sourceKind == PathObjectKind.NotExisting && targetKind == PathObjectKind.NotExisting)
			{
				change.log("not existing");
				return;
			}

			if (sourceKind == PathObjectKind.Ignored || targetKind == PathObjectKind.Ignored)
			{
				change.log("ignored");
				return;
			}

			if (targetKind == PathObjectKind.NotExisting)
			{
				await copyToTarget(change, sourceKind);
				return;
			}
		
			if (sourceKind == PathObjectKind.NotExisting)
			{
				await copyToSource(change, targetKind);
				return;
			}

			if (sourceKind != targetKind)
			{
				change.log("directory <-> file sync not implemented");
				return;
			}

			if (sourceKind == PathObjectKind.Directory)
				await DirectorySynchronizer.syncDirectory(change);
			else
				await FileSynchronizer.syncFile(change, sourceInfo, targetInfo);
		}

		static async Task copyToTarget(PathChange change, PathObjectKind kind)
		{
			switch (kind)
			{
				case PathObjectKind.Directory:
					await DirectorySynchronizer.createDirectory(change, change.Target);
					break;

				case PathObjectKind.File:
					await FileSynchronizer.copyFile(change, change.Source, change.Target);
					break;
			}
		}

		static async Task copyToSource(PathChange change, PathObjectKind kind)
		{
			switch (kind)
			{
				case PathObjectKind.Directory:
					await DirectorySynchronizer.createDirectory(change, change.Source);
					break;

				case PathObjectKind.File:
					await FileSynchronizer.copyFile(change, change.Target, change.Source);
					break;
			}
		}
	}
}
