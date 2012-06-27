using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace InConcert
{
	static class DirectorySynchronizer
	{
		public static async Task syncDirectory(PathChange change)
		{
			var sourceEntries = Directory.GetFileSystemEntries(change.Source);
			var targetEntries = Directory.GetFileSystemEntries(change.Target);

			var all = sourceEntries.Concat(targetEntries).Distinct();

			await Task.WhenAll(all.Select(str => Synchronizer.sync(change.nested(str))));
		}

		public static async Task createDirectory(PathChange change, string path)
		{
			change.log("creating directory");

			if (!change.Configuration.Sync)
				return;

			await Task.Run(() => Directory.CreateDirectory(path));
		}
	}
}
