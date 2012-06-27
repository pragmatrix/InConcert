using System.IO;
using System.Threading.Tasks;
using InConcert.Abstract;

namespace InConcert.System.IO
{
	sealed class WriteFileSystem : IWriteFileSystem
	{
		public Task asyncCopy(string source, string target)
		{
			return Task.Run(() => File.Copy(source, target, overwrite: false));
		}

		public Task asyncOverwrite(string source, string target)
		{
			return Task.Run(() => File.Copy(source, target, overwrite: true));
		}
	}
}
