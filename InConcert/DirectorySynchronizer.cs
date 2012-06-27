using System.Threading.Tasks;
using System.Linq;

namespace InConcert
{
	static class DirectorySynchronizer
	{
		public static async Task syncDirectory(PathChange change)
		{
			if (change.ChangeMode != ChangeMode.Deep)
				return;

			var rfs = change.ReadFileSystem;
			var sourceEntries = rfs.scan(change.Source);
			var targetEntries = rfs.scan(change.Target);

			var all = sourceEntries.Concat(targetEntries).Distinct();

			await Task.WhenAll(all.Select(str => Synchronizer.sync(change.nested(str))));
		}

		public static void createDirectory(PathChange change, string path)
		{
			change.log("creating directory");
			
			change.WriteFileSystem.createDirectory(path);
		}
	}
}
