using InConcert.Abstract;
using Moq;

namespace InConcert.Tests.SynchronizerTests
{
	internal class Shared
	{
		protected const string SourcePath = "s:\\fileordir";
		protected const string TargetPath = "t:\\fileordir";

		protected Mock<IReadFileSystem> _readFileSystem;
		protected Mock<IWriteFileSystem> _writeFileSystem;
		
		protected Mock<IPathInfo> _sourceInfo;
		protected Mock<IPathInfo> _targetInfo;
		
		protected void setup()
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

		internal protected PathChange createSimplePathChange(ChangeLocation location = ChangeLocation.Unknown)
		{
			var config = createSimpleConfiguration();
			return new PathChange(config, ChangeMode.Shallow, location, "fileordir", _readFileSystem.Object, _writeFileSystem.Object);
		}

		Configuration createSimpleConfiguration()
		{
			return Configuration.fromCommandLine("s:\\", "t:\\");
		}
	}
}
