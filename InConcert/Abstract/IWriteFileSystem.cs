using System.Threading.Tasks;

namespace InConcert.Abstract
{
	public interface IWriteFileSystem
	{
		Task copyAsync(string source, string target);
		Task overwriteAsync(string source, string target);
		void createDirectory(string path);
	}
}
