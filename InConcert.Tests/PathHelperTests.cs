using System;
using System.IO;
using NUnit.Framework;

namespace InConcert.Tests
{
	[TestFixture]
	public class PathHelperTests
	{
		[Test]
		public void testRelativePath()
		{
			var cur = Directory.GetCurrentDirectory();
			var relative = "a";
			var expected = Path.Combine(cur, relative);
			var normalized = PathHelper.normalizeSyncPath("a");
			Assert.That(normalized, Is.EqualTo(expected));
		}

		[Test]
		public void testInvalidSyncPath()
		{
			var r = PathHelper.isValidSyncPath("c:\\" + Guid.NewGuid());
			Assert.That(r, Is.False);
		}

		[Test]
		public void testValidSyncPath()
		{
			var r = PathHelper.isValidSyncPath(Directory.GetCurrentDirectory());
			Assert.That(r, Is.True);
		}
	}
}
