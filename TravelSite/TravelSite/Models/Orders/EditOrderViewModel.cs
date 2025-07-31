using TravelSite.Models.Bookings;

namespace TravelSite.Models.Orders
{
	public class EditOrderViewModel
	{
		public Guid Id { get; set; }
		public string Description { get; set; } = string.Empty;
		public DateTime CreateDate { get; set; } 
		public string Status { get; set; } = "";
		public Guid BookingId { get; set; }
	}
}
