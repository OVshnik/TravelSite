using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public class PhotoRepository : IPhotoRepository
	{
		private ApplicationDbContext _context;
		public PhotoRepository(ApplicationDbContext db)
		{
			_context = db;
		}
		public async Task CreatePhotoAsync(TravelPhoto photo)
		{
			_context.TravelPhoto.Add(photo);
			await _context.SaveChangesAsync();
		}
		public async Task DeletePhotoAsync(Guid id)
		{
			var photo = await GetPhotoByIdAsync(id);
			if (photo!=null)
			{
				_context.Remove(photo);
			}
			await _context.SaveChangesAsync();
		}
		public async Task<List<TravelPhoto>> GetAllPhotoAsync()
		{
			var photoList=await _context.TravelPhoto.AsQueryable().
				Include(y => y.Travel).ToListAsync();
			return photoList;
		}
		public async Task<TravelPhoto?> GetPhotoByIdAsync(Guid id)
		{
			var photo=await _context.TravelPhoto.
				Include(y => y.Travel).
				FirstOrDefaultAsync(x=>x.Id==id);
			return photo;
		}
		public async Task UpdatePhotoAsync(TravelPhoto photo)
		{
			_context.Update(photo);
			await _context.SaveChangesAsync();
		}
	}
}
