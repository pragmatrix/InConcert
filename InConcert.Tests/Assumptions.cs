using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace InConcert.Tests
{
	[TestFixture]
	static class Assumptions
	{
		[Test]
		public static void canTestForExistingDirectoryWithFileInfo()
		{
			var curDir = Directory.GetCurrentDirectory();
			var fi = new FileInfo(curDir);
			Assert.That(fi.Exists, Is.False);
			Assert.That(fi.Attributes != (FileAttributes)(-1));
			Assert.That((fi.Attributes & FileAttributes.Directory), Is.EqualTo(FileAttributes.Directory));
		}

		[Test]
		public static void canTestForNotExistingPathWithFileInfo()
		{
			var curDir = Directory.GetCurrentDirectory();
			curDir = Path.Combine(curDir, Guid.NewGuid().ToString());
			var fi = new FileInfo(curDir);
			Assert.That(fi.Exists, Is.False);
			Assert.That(fi.Attributes, Is.EqualTo((FileAttributes)(-1)));
		}

		[Test]
		public static void directoryGetFileSystemEntriesReturnsFullPathNames()
		{
			var safeFoldertoScan = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			var entries = Directory.GetFileSystemEntries(safeFoldertoScan);
			Assert.True(entries.Any());
			Assert.True(entries.All(Path.IsPathRooted));
		}

		[Test, ExpectedException(typeof(IOException))]
		public static void copyWhenFileAlreadyExists()
		{
			var tmpFilename1 = Path.GetTempFileName();
			File.WriteAllBytes(tmpFilename1, new byte[0]);

			var tmpFilename2 = Path.GetTempFileName();
			File.WriteAllBytes(tmpFilename2, new byte[0]);

			File.Copy(tmpFilename1, tmpFilename2);
		}

		[Test]
		public static void createDirectoryThatExists()
		{
			var cur = Directory.GetCurrentDirectory();
			Directory.CreateDirectory(cur);
		}
	}
}
