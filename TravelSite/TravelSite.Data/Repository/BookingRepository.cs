using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public class BookingRepository : IBookingRepository
	{
		private ApplicationDbContext _context;
		public BookingRepository(ApplicationDbContext db)
		{
			_context = db;
		}
		public async Task CreateBookingAsync(Booking booking)
		{
			_context.Entry(booking).State = EntityState.Added;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteBookingAsync(Guid id)
		{
			var book = await GetBookingByIdAsync(id);
			if (book != null)
			{
				_context.Bookings.Remove(book);
			}
			await _context.SaveChangesAsync();
		}

		public async Task<List<Booking>> GetAllBookingsAsync()
		{
			var bookings = await _context.Bookings.AsQueryable().
				Include(x => x.Travel).
				Include(y => y.User).
				Include(z => z.Order).ToListAsync();
			return bookings;
		}

		public async Task<Booking?> GetBookingByIdAsync(Guid id)
		{
			var booking = await _context.Bookings.Where(x => x.Id == id).
				Include(x => x.Travel).
                Include(y => y.User).
				Include(z => z.Order).AsNoTracking().FirstOrDefaultAsync();
			return booking;
		}

		public async Task UpdateBookingAsync(Booking booking)
		{
			_context.Update(booking);
			await _context.SaveChangesAsync();
		}
	}
}
