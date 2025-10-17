namespace TravelSite.Services
{
	public interface IFileService
	{
		public Task<string> SaveFileInFolder(IFormFile formFile, string path, string subFileName);
		public Task SaveFileInFolder(string path, string content, string extension, string fileName);
		public Task<string> ReadFileInFolder(string path, string extension, string fileName);
		public void DeleteFileInFolder(string path, string fileName);
	}
}
