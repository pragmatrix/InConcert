using System.IO;

namespace InConcert
{
	enum PathObjectKind
	{
		File,
		Directory,
		Ignored,
		NotExisting
	}

	static class FileInfoExtensions
	{
		public static bool isExistingPath(this FileInfo info)
		{
			return info.Attributes != (FileAttributes) (-1);
		}

		public static PathObjectKind getKindOf(this FileInfo info)
		{
			var attributes = info.Attributes;
			if (attributes == (FileAttributes)(-1))
				return PathObjectKind.NotExisting;

			if ((attributes & IgnoreFileMask) != 0)
				return PathObjectKind.Ignored;

			return (attributes & FileAttributes.Directory) != 0 
				? PathObjectKind.Directory 
				: PathObjectKind.File;
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
	}
}
