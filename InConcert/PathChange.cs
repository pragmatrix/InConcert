using System.IO;
using InConcert.Abstract;
using Console = System.Console;

namespace InConcert
{
	sealed class PathChange
	{
		public readonly Configuration Configuration;
		public readonly ChangeLocation ChangeLocation;
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
			ChangeLocation changeLocation, 
			string relativePath,
			IReadFileSystem readFileSystem,
			IWriteFileSystem writeFileSystem)
		{
			Configuration = configuration;
			ChangeLocation = changeLocation;
			RelativePath = relativePath;
			ReadFileSystem = readFileSystem;
			WriteFileSystem = writeFileSystem;

			Source = Path.Combine(Configuration.SourcePath, RelativePath);
			Target = Path.Combine(Configuration.TargetPath, RelativePath);
		}

		public PathChange nested(string name)
		{
			return new PathChange(Configuration,
				ChangeLocation,
				Path.Combine(RelativePath, name),
				ReadFileSystem,
				WriteFileSystem);
		}
	}
}