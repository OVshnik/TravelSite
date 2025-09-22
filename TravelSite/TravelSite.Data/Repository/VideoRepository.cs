using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public class VideoRepository:IVideoRepository
	{
		private ApplicationDbContext _context;
		public VideoRepository(ApplicationDbContext db)
		{
			_context = db;
		}
		public async Task CreateVideoAsync(TravelVideo video)
		{
			_context.TravelVideo.Add(video);
			await _context.SaveChangesAsync();
		}
		public async Task DeleteVideoAsync(Guid id)
		{
			var video = await GetVideoByIdAsync(id);
			if (video != null)
			{
				_context.Remove(video);
			}
			await _context.SaveChangesAsync();
		}
		public async Task<List<TravelVideo>> GetAllVideoAsync()
		{
			var videoList = await _context.TravelVideo.AsQueryable().
				Include(y => y.Travel).ToListAsync();
			return videoList;
		}
		public async Task<TravelVideo?> GetVideoByIdAsync(Guid id)
		{
			var video = await _context.TravelVideo.
				Include(y => y.Travel).
				FirstOrDefaultAsync(x => x.Id == id);
			return video;
		}
		public async Task UpdateVideoAsync(TravelVideo video)
		{
			_context.Update(video);
			await _context.SaveChangesAsync();
		}
	}
}
