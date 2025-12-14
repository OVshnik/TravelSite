using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public interface IBookingRepository
	{
		Task CreateBookingAsync(Booking booking);
		Task<List<Booking>> GetAllBookingsAsync();
		Task<Booking?> GetBookingByIdAsync(Guid id);
		Task UpdateBookingAsync(Booking booking);
		Task DeleteBookingAsync(Guid id);
	}
}
