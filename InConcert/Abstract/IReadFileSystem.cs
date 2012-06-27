namespace InConcert.Abstract
{
	interface IReadFileSystem
	{
		IPathInfo query(string path);
		IReadStream open(string path);
	}
}
