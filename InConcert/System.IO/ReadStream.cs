using System.IO;
using System.Threading.Tasks;
using InConcert.Abstract;

namespace InConcert.System.IO
{
	sealed class ReadStream : IReadStream
	{
		readonly Stream _stream;

		public ReadStream(string path)
		{
			_stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite);
		}

		public Task<int> readAsync(byte[] buf, int offset, int count)
		{
			return _stream.ReadAsync(buf, offset, count);
		}
	}
}
