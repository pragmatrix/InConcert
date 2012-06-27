using System.IO;
using System.Threading.Tasks;
using InConcert.Abstract;

namespace InConcert.System.IO
{
	sealed class WriteFileSystem : IWriteFileSystem
	{
		public void createDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		public void deleteDirectoryRecursive(string path)
		{
			Directory.Delete(path, true);
		}

		public Task copyAsync(string source, string target)
		{
			return Task.Run(() => File.Copy(source, target, overwrite: false));
		}

		public Task overwriteAsync(string source, string target)
		{
			return Task.Run(() => File.Copy(source, target, overwrite: true));
		}

		public void deleteFile(string path)
		{
			File.Delete(path);
		}
	}
}
