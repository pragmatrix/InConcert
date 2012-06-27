using System.Threading.Tasks;

namespace InConcert.Abstract
{
	public interface IWriteFileSystem
	{
		void createDirectory(string path);
		void deleteDirectoryRecursive(string path);

		Task copyAsync(string source, string target);
		Task overwriteAsync(string source, string target);
		void deleteFile(string path);
	}
}
