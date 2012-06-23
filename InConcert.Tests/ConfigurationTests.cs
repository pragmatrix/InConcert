using System;
using NUnit.Framework;

namespace InConcert.Tests
{
	[TestFixture]
    public class ConfigurationTests
    {
		[Test, ExpectedException(typeof(Exception))]
		public void testBare()
		{
			Configuration.fromCommandLine();
		}

		[Test]
		public void testWatch()
		{
			var config = Configuration.fromCommandLine("a","b", "--watch");
			Assert.That(config.Watch, Is.True);
		}

		[Test]
		public void testSync()
		{
			var config = Configuration.fromCommandLine("a", "b", "--sync");
			Assert.That(config.Sync, Is.True);
		}

		[Test]
		public void testPaths()
		{
			var config = Configuration.fromCommandLine("a", "b");
			Assert.That(config.SourcePath, Is.EqualTo("a"));
			Assert.That(config.TargetPath, Is.EqualTo("b"));
		}

		[Test]
		public void testNoOptions()
		{
			var config = Configuration.fromCommandLine("a", "b");
			Assert.That(config.Git, Is.False);
			Assert.That(config.Watch, Is.False);
			Assert.That(config.Sync, Is.False);
		}

		[Test]
		public void testOptionsPrefixed()
		{
			var config = Configuration.fromCommandLine("--watch", "a", "b");
			Assert.That(config.Watch, Is.True);
			Assert.That(config.SourcePath, Is.EqualTo("a"));
			Assert.That(config.TargetPath, Is.EqualTo("b"));
		}
    }
}
