using System;
using System.Collections.Generic;

namespace InConcert.Abstract
{
	public interface IReadFileSystem
	{
		IDisposable beginWatch(string path, Action<string> asyncChanged);

		IPathInfo query(string path);
		IReadStream open(string path);
		IEnumerable<string> scan(string path);
	}
}
