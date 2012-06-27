using System;
using System.IO;
using InConcert.Abstract;

namespace InConcert.System.IO
{
	sealed class PathInfo : IPathInfo
	{
		readonly FileInfo _info;

		public PathInfo(string path)
		{
			_info = new FileInfo(path);
		}

#if false
		public static bool isExistingPath(this FileInfo info)
		{
			return info.Attributes != (FileAttributes) (-1);
		}
#endif

		public PathType Type
		{
			get
			{
				var attributes = _info.Attributes;
				if (attributes == (FileAttributes) (-1))
					return PathType.NotExisting;

				if ((attributes & IgnoreFileMask) != 0)
					return PathType.Ignored;

				return (attributes & FileAttributes.Directory) != 0
					? PathType.Directory
					: PathType.File;
			}
		}

		const FileAttributes IgnoreFileMask = 
			  FileAttributes.Temporary
			| FileAttributes.System
			| FileAttributes.Encrypted
			| AvoidScanNestedMask ;

		const FileAttributes AvoidScanNestedMask =
			FileAttributes.Device
			| FileAttributes.ReparsePoint
			| FileAttributes.Offline;

		public long Length
		{
			get { return _info.Length; }
		}

		public DateTime LastWriteTimeUtc
		{
			get { return _info.LastWriteTimeUtc; }
		}
	}
}
