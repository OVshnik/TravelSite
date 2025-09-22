namespace TravelSite.Services
{
	public interface IFileService
	{
		public Task<string> SaveFileInFolder(IFormFile formFile, string path, string subFileName);
		public void DeleteFileInFolder(string path, string fileName);
	}
}
