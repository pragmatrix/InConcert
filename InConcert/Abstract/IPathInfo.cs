using System;

namespace InConcert.Abstract
{
	enum PathType
	{
		File,
		Directory,
		Ignored,
		NotExisting
	}

	interface IPathInfo
	{
		PathType Type { get; }

		long Length { get; }
		DateTime LastWriteTimeUtc { get; }
	}
}
