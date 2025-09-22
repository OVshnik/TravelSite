
using Microsoft.AspNetCore.Http;
using TravelSite.Data.Models;

namespace TravelSite.Services
{
	public class FileService : IFileService
	{ 
		public async Task<string> SaveFileInFolder(IFormFile formFile,string path, string subFileName)
		{
			var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();

			if (!Directory.Exists(path))
			{
				var b = Directory.CreateDirectory(path);
			}

			var fileName = $"{subFileName}_{Path.GetRandomFileName()}{extension}";
			var filePath = Path.Combine(path, fileName);

			using var stream = System.IO.File.Create(filePath);
			await formFile.CopyToAsync(stream);

			return fileName;
		}
		public void DeleteFileInFolder(string path,string fileName)
		{
			var filePath = Path.Combine(path, fileName);

			using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				File.Delete(filePath);
			}
		}
	}
}
