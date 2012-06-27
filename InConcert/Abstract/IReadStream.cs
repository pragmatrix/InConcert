using System.Threading.Tasks;

namespace InConcert.Abstract
{
	interface IReadStream
	{
		Task<int> readAsync(byte[] buf, int offset, int count);
	}
}
