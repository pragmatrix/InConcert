using System;
using System.IO;
using Toolbox;

namespace InConcert
{
	static class PathHelper
	{
		public static string importSyncPath(string path)
		{
			var normalized = normalizeSyncPath(path);
			if (!isValidSyncPath(normalized))
				throw new Exception("{0}: is not a valid synchornization path, please ensure that the directory is existing".format(normalized));

			return normalized;
		}

		internal static string normalizeSyncPath(string path)
		{
			return Path.GetFullPath(path);
		}

		internal static bool isValidSyncPath(string syncPath)
		{
			return Directory.Exists(syncPath);
		}
	}
}
