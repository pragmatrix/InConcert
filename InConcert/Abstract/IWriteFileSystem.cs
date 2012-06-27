using System.Threading.Tasks;

namespace InConcert.Abstract
{
	public interface IWriteFileSystem
	{
		Task asyncCopy(string source, string target);
		Task asyncOverwrite(string source, string target);
		void createDirectory(string path);
	}
}
