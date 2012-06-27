namespace InConcert.Abstract
{
	public interface IReadFileSystem
	{
		IPathInfo query(string path);
		IReadStream open(string path);
	}
}
