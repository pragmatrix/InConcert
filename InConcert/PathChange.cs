using System.IO;

namespace InConcert
{
	sealed class PathChange
	{
		public readonly Configuration Configuration;
		public readonly ChangeLocation ChangeLocation;
		public readonly string RelativePath;
		public readonly string Source;
		public readonly string Target;

		public void log(string str)
		{
			System.Console.WriteLine(RelativePath + ": " + str);
		}

		public PathChange(Configuration configuration, ChangeLocation changeLocation, string relativePath)
		{
			Configuration = configuration;
			ChangeLocation = changeLocation;
			RelativePath = relativePath;

			Source = Path.Combine(Configuration.SourcePath, RelativePath);
			Target = Path.Combine(Configuration.TargetPath, RelativePath);
		}

		public PathChange nested(string name)
		{
			return new PathChange(Configuration,
				ChangeLocation,
				Path.Combine(RelativePath, name));
		}
	}
}