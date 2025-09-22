using System.Security.Claims;
using TravelSite.Models.Bookings;
namespace TravelSite.Services
{
	public interface IBookingService
	{
		public Task<CreateBookingViewModel> AddBookingAsync(Guid id, ClaimsPrincipal claims);
		public Task<Guid> AddBookingAsync(CreateBookingViewModel model);
		public Task RemoveBookingAsync(Guid id);
		public Task <BookingViewModel> GetBookingAsync(Guid id);
		public Task <List<BookingViewModel>> GetAllBookingAsync();
		public Task<EditBookingViewModel> EditBookingAsync(Guid id);
		public Task UpdateBookingAsync(EditBookingViewModel model);
		public Task ConfirmBooking(Guid id, string senderId, string bookNum);
		public Task CancelBooking(Guid id, string senderId, string bookNum);
		public Task<string> GenerateBookingNumber(string trName);

	}
}
