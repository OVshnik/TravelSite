using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public class TravelDatesRepository : ITravelDatesRepository
	{
		private ApplicationDbContext _context;
		public TravelDatesRepository(ApplicationDbContext db)
		{
			_context = db;
		}
		public async Task CreateTravelDatesAsync(TravelDates dates)
		{
			_context.Entry(dates).State = EntityState.Added;
			await _context.SaveChangesAsync();
		}
		public async Task DeleteTravelDatesAsync(Guid id)
		{
			var date = await GetTravelDatesByIdAsync(id);
			if (date!=null)
			{
				_context.Remove(date);
			}
			await _context.SaveChangesAsync();
		}
		public async Task<List<TravelDates>> GetAllTravelDatesAsync()
		{
			var dates=await _context.TravelDates.ToListAsync();
			return dates;
		}
		public async Task<TravelDates?> GetTravelDatesByIdAsync(Guid id)
		{
			var date=await _context.TravelDates.Where(x=> x.Id == id).FirstOrDefaultAsync();
			return date;
		}
		public async Task UpdateTravelDatesAsync(TravelDates dates)
		{
			_context.Update(dates);
			await _context.SaveChangesAsync();
		}
	}
}
