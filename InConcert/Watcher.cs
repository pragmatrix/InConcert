using System;
using System.IO;
using Toolbox;

namespace InConcert
{
	sealed class Watcher : IDisposable
	{
		readonly string _path;
		readonly Action<string> _receiver;

		const int InitialBufferSize = 0x8000;

		readonly FileSystemWatcher _watcher;

		public Watcher(string path, Action<string> receiver)
		{
			_path = path;
			_receiver = receiver;

			_watcher = new FileSystemWatcher();
			_watcher.BeginInit();
			_watcher.Filter = string.Empty;
			_watcher.IncludeSubdirectories = true;
			_watcher.InternalBufferSize = InitialBufferSize;
			_watcher.Path = path;

			_watcher.NotifyFilter
				= NotifyFilters.DirectoryName
				| NotifyFilters.FileName
				| NotifyFilters.LastWrite

				// we want attributes to catch error-fixups

				| NotifyFilters.Attributes
				| NotifyFilters.Security;
			// we assume that size changes are covered by LastWrite 
			// | NotifyFilters.Size;

			// note: all events are on threadpool threads!
			_watcher.Created += onCreated;
			_watcher.Deleted += onDeleted;
			_watcher.Changed += onChanged;
			_watcher.Renamed += onRenamed;
			_watcher.Error += onError;

			_watcher.EndInit();

			_watcher.EnableRaisingEvents = true;
		}

		void onCreated(object sender, FileSystemEventArgs e)
		{
			_receiver(stripPath(e.FullPath));
		}

		void onDeleted(object sender, FileSystemEventArgs e)
		{
			_receiver(stripPath(e.FullPath));
		}

		void onChanged(object sender, FileSystemEventArgs e)
		{
			_receiver(stripPath(e.FullPath));
		}

		void onRenamed(object sender, RenamedEventArgs e)
		{
			_receiver(stripPath(e.OldFullPath));
			_receiver(stripPath(e.FullPath));
		}

		void onError(object sender, ErrorEventArgs e)
		{
			throw new Exception("{0}: File system watcher error {1}".format(_path, e.GetException().Message));
		}

		string stripPath(string path)
		{
			if (!path.StartsWith(_path))
				throw new Exception("{0}: internal error, invalid changeed path: {1}".format(_path, path));
	
			return path.Substring(_path.Length);
		}

		public void Dispose()
		{
			_watcher.EnableRaisingEvents = false;
			_watcher.Dispose();
		}
	}
}
