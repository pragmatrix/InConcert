using System;
using System.Threading.Tasks;
using InConcert.Abstract;
using Moq;
using NUnit.Framework;

namespace InConcert.Tests.SynchronizerTests
{
	[TestFixture]
	public class FileSynchronization
	{
		Mock<IReadFileSystem> _readFileSystem;
		Mock<IWriteFileSystem> _writeFileSystem;

		Mock<IPathInfo> _sourceInfo;
		Mock<IPathInfo> _targetInfo;

		const string SourcePath = "s:\\file.txt";
		const string TargetPath = "t:\\file.txt";


		[Test]
		public void testSameFilesDontSynchronize()
		{
			setupSameFiles();

			var pc = createSimplePathChange();

			FileSynchronizer.syncFile(pc, _sourceInfo.Object, _targetInfo.Object)
				.Wait();
		}

		[Test]
		public void testSameFilesWithDifferentDatesDontSynchronize()
		{
			setupSameFiles();
			setupNewerTarget();

			var pc = createSimplePathChange();

			FileSynchronizer.syncFile(pc, _sourceInfo.Object, _targetInfo.Object)
				.Wait();
		}

		[Test]
		public void testDifferentFilesSynchronize()
		{
			setupDifferentFiles();
			setupOverwrite();

			var pc = createSimplePathChange();

			FileSynchronizer.syncFile(pc, _sourceInfo.Object, _targetInfo.Object)
				.Wait();
			// in this test, the change source is unknown and the date time is the same, so we expect a copy from source to target (with overwrite!)

			_writeFileSystem.Verify(ifs => ifs.overwriteAsync(SourcePath, TargetPath), Times.Once());
		}

		[Test]
		public void testDifferentFilesWithDifferentDates()
		{
			setupDifferentFiles();
			setupOverwrite();
			setupNewerTarget();

			var pc = createSimplePathChange();

			FileSynchronizer.syncFile(pc, _sourceInfo.Object, _targetInfo.Object)
				.Wait();
			// change location is unknown, but target is newer, so we prefer target.
			_writeFileSystem.Verify(ifs => ifs.overwriteAsync(TargetPath, SourcePath), Times.Once());
		}

		[Test]
		public void testDifferentFilesChangedAtTarget()
		{
			setupDifferentFiles();
			setupOverwrite();

			var pc = createSimplePathChange(ChangeLocation.AtTarget);
			FileSynchronizer.syncFile(pc, _sourceInfo.Object, _targetInfo.Object)
				.Wait();

			_writeFileSystem.Verify(ifs => ifs.overwriteAsync(TargetPath, SourcePath), Times.Once());
		}

		[SetUp]
		public void setup()
		{
			_readFileSystem = new Mock<IReadFileSystem>();
			_writeFileSystem = new Mock<IWriteFileSystem>();

			_sourceInfo = new Mock<IPathInfo>();
			_targetInfo = new Mock<IPathInfo>();

			_sourceInfo.SetupGet(si => si.Type).Returns(PathType.File);
			_targetInfo.SetupGet(si => si.Type).Returns(PathType.File);

			_readFileSystem.Setup(rfs => rfs.query(SourcePath)).Returns(_sourceInfo.Object);
			_readFileSystem.Setup(rfs => rfs.query(TargetPath)).Returns(_targetInfo.Object);
		}

		void setupOverwrite()
		{
			_writeFileSystem.Setup(wfs => wfs.overwriteAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.Run(() => { }));
		}

		PathChange createSimplePathChange(ChangeLocation location = ChangeLocation.Unknown)
		{
			var config = createSimpleConfiguration();
			return new PathChange(config, ChangeMode.Shallow, location, "file.txt", _readFileSystem.Object, _writeFileSystem.Object);
		}

		Configuration createSimpleConfiguration()
		{
			return Configuration.fromCommandLine("s:\\", "t:\\");
		}


		void setupSameFiles()
		{
			setupSourceBuffer(new byte[] { 1 });
			setupTargetBuffer(new byte[] { 1 });
		}

		void setupDifferentFiles()
		{
			setupSourceBuffer(new byte[] {1});
			setupTargetBuffer(new byte[] {0});
		}

		void setupSourceBuffer(byte[] buf)
		{
			_readFileSystem.Setup(rfs => rfs.open(SourcePath)).Returns(new ReadFileStream(buf));
		}

		void setupTargetBuffer(byte[] buf)
		{
			_readFileSystem.Setup(rfs => rfs.open(TargetPath)).Returns(new ReadFileStream(buf));
		}

		void setupNewerTarget()
		{
			setupDate(_sourceInfo, new DateTime(2001, 1, 1));
			// target is newer and ChangeLocation is unknown
			setupDate(_targetInfo, new DateTime(2002, 2, 2));
		}

		void setupDate(Mock<IPathInfo> mock, DateTime dt)
		{
			mock.SetupGet(pi => pi.LastWriteTimeUtc).Returns(dt);
		}

		sealed class ReadFileStream : IReadStream
		{
			readonly byte[] _buf;
			bool _eof;

			public ReadFileStream(byte[] buf)
			{
				_buf = buf;
			}

			public Task<int> readAsync(byte[] buf, int offset, int count)
			{

				return Task.Run(() =>
					{
						if (_eof)
							return 0;

						Assert.True(offset == 0);
						Assert.True(_buf.Length <= count);
						Array.Copy(_buf, buf, _buf.Length);
						_eof = true;
						return _buf.Length;
					});
			}
		}
	}
}
