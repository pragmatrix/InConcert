using System.Collections.Generic;

namespace InConcert.Abstract
{
	public interface IReadFileSystem
	{
		IPathInfo query(string path);
		IReadStream open(string path);
		IEnumerable<string> scan(string path);
	}
}
