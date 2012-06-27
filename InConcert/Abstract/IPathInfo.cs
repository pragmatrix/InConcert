using System;

namespace InConcert.Abstract
{
	public enum PathType
	{
		File,
		Directory,
		Ignored,
		NotExisting
	}

	public interface IPathInfo
	{
		PathType Type { get; }

		long Length { get; }
		DateTime LastWriteTimeUtc { get; }
	}
}
