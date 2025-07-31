using TravelSite.Data.Models;
using TravelSite.Models.Bookings;
using TravelSite.Models.Orders;

namespace TravelSite.Models.Account
{
    public class UserViewModel
    {
		public User User { get; set; }
		public UserViewModel(User user)
		{
			User = user;
		}
		public List<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();
		public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
	}
}
