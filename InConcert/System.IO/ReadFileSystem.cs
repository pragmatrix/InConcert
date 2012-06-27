using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InConcert.Abstract;

namespace InConcert.System.IO
{
	sealed class ReadFileSystem : IReadFileSystem
	{
		public IDisposable beginWatch(string path, Action<string> asyncChanged)
		{
			return new Watcher(path, asyncChanged);
		}

		public IPathInfo query(string path)
		{
			return new PathInfo(path);
		}

		public IReadStream open(string path)
		{
			return new ReadStream(path);
		}

		public IEnumerable<string> scan(string path)
		{
			return filenames(Directory.GetFileSystemEntries(path));
		}

		static IEnumerable<string> filenames(IEnumerable<string> paths)
		{
			return paths.Select(Path.GetFileName);
		}
	}
}
