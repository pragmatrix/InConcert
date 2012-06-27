using System;
using System.Threading.Tasks;

namespace InConcert.Abstract
{
	public interface IReadStream : IDisposable
	{
		Task<int> readAsync(byte[] buf, int offset, int count);
	}
}
