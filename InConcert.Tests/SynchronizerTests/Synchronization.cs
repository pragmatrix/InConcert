using System.Threading.Tasks;
using InConcert.Abstract;
using Moq;
using NUnit.Framework;

namespace InConcert.Tests.SynchronizerTests
{
	[TestFixture]
	class Synchronization : Shared
	{
		[Test]
		public void copyIfNoTarget()
		{
			setupNoTarget();
			setupCopy();

			var pc = createSimplePathChange();
			syncPathChange(pc);

			_writeFileSystem.Verify(wfs => wfs.copyAsync(SourcePath, TargetPath), Times.Once());
		}

		[Test]
		public void deleteSourceIfNoTargetButLocationIsTarget()
		{
			setupNoTarget();
			setupDelete();

			var pc = createSimplePathChange(location: ChangeLocation.AtTarget);
			syncPathChange(pc);

			_writeFileSystem.Verify(wfs => wfs.deleteFile(SourcePath), Times.Once());
		}

		[Test]
		public void deleteSourceDirIfNoTargetButLocationIsTarget()
		{
			setupNoTarget();
			setupDelete();
			_sourceInfo.SetupGet(si => si.Type).Returns(PathType.Directory);

			var pc = createSimplePathChange(location: ChangeLocation.AtTarget);
			syncPathChange(pc);

			_writeFileSystem.Verify(wfs => wfs.deleteDirectoryRecursive(SourcePath), Times.Once());
		}

		[Test]
		public void createDirectoryIfNoTarget()
		{
			_sourceInfo.SetupGet(si => si.Type).Returns(PathType.Directory);
			_targetInfo.SetupGet(si => si.Type).Returns(PathType.NotExisting);

			var pc = createSimplePathChange();
			syncPathChange(pc);

			_writeFileSystem.Verify(wfs => wfs.createDirectory(TargetPath), Times.Once());
		}


		[SetUp]
		public new void setup()
		{
			base.setup();
		}

		void setupCopy()
		{
			_writeFileSystem.Setup(wfs => wfs.copyAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.Run(() => { }));
		}

		void setupDelete()
		{
			_writeFileSystem.Setup(wfs => wfs.deleteFile(It.IsAny<string>()));
		}

		void setupNoTarget()
		{
			_targetInfo.SetupGet(si => si.Type).Returns(PathType.NotExisting);
		}

		void syncPathChange(PathChange pc)
		{
			Synchronizer.sync(pc)
				.Wait();
		}
	}
}
