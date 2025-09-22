using System.ComponentModel.DataAnnotations;
using TravelSite.Data.Models;
using TravelSite.Models.Account;
using TravelSite.Models.Orders;
using TravelSite.Models.TravelDates;
using TravelSite.Models.Travels;
using TravelSite.Validation;

namespace TravelSite.Models.Bookings
{
	public class CreateBookingViewModel
	{
		public DateTime BookDate { get; set; }
		public string BookingStatus { get; set; } = "";
		public string BookingNumber { get; set; } = "";
		public DateOnly From {  get; set; }
		public DateOnly To { get; set; }
		public int Price { get; set; }
		[CheckedDatesAmount]
		public List <TravelDatesViewModel> TravelDates { get; set; }= new List<TravelDatesViewModel>();
		public string UserId { get; set; } = "";
		public TravelViewModel? Travel { get; set; }
	}
}
