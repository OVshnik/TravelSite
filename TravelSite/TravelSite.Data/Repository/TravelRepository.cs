using Microsoft.EntityFrameworkCore;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public class TravelRepository : ITravelRepository
	{
		private ApplicationDbContext _context;
		public TravelRepository(ApplicationDbContext db)
		{
			_context = db;
		}
		public async Task CreateTravelAsync(Travel prod)
		{
			_context.Travels.Add(prod);
			await _context.SaveChangesAsync();
		}
		public async Task DeleteTravelAsync(Guid id)
		{
			var prod = await GetTravelByIdAsync(id);
			if (prod!=null)
			{
				_context.Remove(prod);
			}
			await _context.SaveChangesAsync();
		}
		public async Task<List<Travel>> GetAllTravelsAsync()
		{
			var products=await _context.Travels.AsQueryable().
				Include(x=>x.TravelDates).
				Include(y=>y.BookingList).
				Include(z=>z.UserList).ToListAsync();
			return products;
		}
		public async Task<Travel?> GetTravelByIdAsync(Guid id)
		{
			var prod=await _context.Travels.
				Include(x => x.TravelDates).
				Include(y => y.BookingList).
				Include(z => z.UserList).
				FirstOrDefaultAsync(x=>x.Id==id);
			return prod;
		}
		public async Task UpdateTravelAsync(Travel prod)
		{
			_context.Update(prod);
			await _context.SaveChangesAsync();
		}
	}
}
