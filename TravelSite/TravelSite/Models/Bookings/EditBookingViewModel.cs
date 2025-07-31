using TravelSite.Data.Models;

namespace TravelSite.Models.Bookings
{
	public class EditBookingViewModel
	{
		public Guid Id { get; set; }
		public DateTime BookDate { get; set; }
		public DateOnly From { get; set; }
		public DateOnly To { get; set; }
		public int Price { get; set; }
		public string BookingStatus { get; set; } = "";
		public User? User { get; set; }
		public Travel? Travel { get; set; }
		public Order? Order { get; set; }
	}
}
