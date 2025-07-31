using TravelSite.Data.Models;
using TravelSite.Models.Bookings;
using TravelSite.Models.Travels;

namespace TravelSite.Models.Orders
{
	public class CreateOrderViewModel
	{
		public Guid Id { get; set; }
		public string OrderNumber { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public string Status { get; set; } = "Unconfirmed";
		public int TotalPrice { get; set; }
		public Guid BookingId { get; set; }
	    public TravelViewModel? Travel { get; set; }
	}
}
