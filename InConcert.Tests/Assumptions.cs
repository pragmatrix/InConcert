using System;
using System.IO;
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
		public static void canTestForNotExistingDirectoryWithFileInfo()
		{
			var curDir = Directory.GetCurrentDirectory();
			curDir = Path.Combine(curDir, Guid.NewGuid().ToString());
			var fi = new FileInfo(curDir);
			Assert.That(fi.Exists, Is.False);
			Assert.That(fi.Attributes, Is.EqualTo((FileAttributes)(-1)));
		}
	}
}
