using InConcert.Abstract;

namespace InConcert.System.IO
{
	sealed class ReadFileSystem : IReadFileSystem
	{
		public IPathInfo query(string path)
		{
			return new PathInfo(path);
		}

		public IReadStream open(string path)
		{
			return new ReadStream(path);
		}
	}
}
