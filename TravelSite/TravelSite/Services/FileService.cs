
using Microsoft.AspNetCore.Http;
using TravelSite.Data.Models;

namespace TravelSite.Services
{
	public class FileService : IFileService
	{
		public async Task<string> SaveFileInFolder(IFormFile formFile, string path, string subFileName)
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
		public async Task SaveFileInFolder(string path, string content, string extension, string fileName)
		{

			if (!Directory.Exists(path))
			{
				var b = Directory.CreateDirectory(path);
			}

			fileName = fileName + extension;

			path = Path.Combine(path, fileName);

			using (StreamWriter writer = new StreamWriter(path, false))
			{
				await writer.WriteAsync(content);
			}
		}
		public async Task<string> ReadFileInFolder(string path, string extension, string fileName)
		{
			fileName = fileName + extension;

			path = Path.Combine(path, fileName);

			using (StreamReader writer = new StreamReader(path, false))
			{
				var content=await writer.ReadLineAsync();
				if(content != null)
				return content;
				else
				return String.Empty;
			}
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
