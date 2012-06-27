using System.Threading.Tasks;
using InConcert.Abstract;

namespace InConcert
{
	static class Synchronizer
	{
		public static async Task sync(PathChange change)
		{
			var reader = change.ReadFileSystem;
			var sourceInfo = reader.query(change.Source);
			var targetInfo = reader.query(change.Target);

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
				if (change.Location == ChangeLocation.AtTarget)
					deleteSource(change, sourceKind);
				else
					await copyToTarget(change, sourceKind);
				return;
			}
		
			if (sourceKind == PathType.NotExisting)
			{
				if (change.Location == ChangeLocation.AtSource)
					deleteTarget(change, targetKind);
				else
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

		static void deleteSource(PathChange change, PathType type)
		{
			delete(change, change.Source, type);
		}

		static void deleteTarget(PathChange change, PathType type)
		{
			delete(change, change.Target, type);
		}

		static void delete(PathChange change, string path, PathType type)
		{
			var writer = change.WriteFileSystem;
			switch (type)
			{
				case PathType.Directory:
					writer.deleteDirectoryRecursive(path);
					break;
				case PathType.File:
					writer.deleteFile(path);
					break;
			}
		}
	}
}
