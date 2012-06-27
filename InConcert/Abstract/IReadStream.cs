using System.Threading.Tasks;

namespace InConcert.Abstract
{
	public interface IReadStream
	{
		Task<int> readAsync(byte[] buf, int offset, int count);
	}
}
