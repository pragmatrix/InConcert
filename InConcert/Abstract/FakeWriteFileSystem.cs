using System.Threading.Tasks;

namespace InConcert.Abstract
{
	sealed class FakeWriteFileSystem : IWriteFileSystem
	{
		public void createDirectory(string path)
		{
		}

		public void deleteDirectoryRecursive(string path)
		{
		}

		public async Task copyAsync(string source, string target)
		{
		}

		public async Task overwriteAsync(string source, string target)
		{
		}

		public void deleteFile(string path)
		{
		}

	}
}
