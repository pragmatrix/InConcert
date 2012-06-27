using System.IO;
using InConcert.Abstract;
using Console = System.Console;

namespace InConcert
{
	enum ChangeMode
	{
		Shallow,
		Deep
	}

	enum ChangeLocation
	{
		AtSource,
		AtTarget,
		Unknown
	}

	sealed class PathChange
	{
		public readonly Configuration Configuration;
		public readonly ChangeMode ChangeMode;
		public readonly ChangeLocation Location;
		public readonly string RelativePath;
		public readonly string Source;
		public readonly string Target;
		public readonly IReadFileSystem ReadFileSystem;
		public readonly IWriteFileSystem WriteFileSystem;


		public void log(string str)
		{
			Console.WriteLine(RelativePath + ": " + str);
		}

		public PathChange(
			Configuration configuration, 
			ChangeMode mode,
			ChangeLocation location, 
			string relativePath,
			IReadFileSystem readFileSystem,
			IWriteFileSystem writeFileSystem)
		{
			Configuration = configuration;
			ChangeMode = mode;
			Location = location;
			RelativePath = relativePath;
			ReadFileSystem = readFileSystem;
			WriteFileSystem = writeFileSystem;

			Source = Path.Combine(Configuration.SourcePath, RelativePath);
			Target = Path.Combine(Configuration.TargetPath, RelativePath);
		}

		public PathChange nested(string name)
		{
			return new PathChange(Configuration,
				ChangeMode,
				Location,
				Path.Combine(RelativePath, name),
				ReadFileSystem,
				WriteFileSystem);
		}
	}
}