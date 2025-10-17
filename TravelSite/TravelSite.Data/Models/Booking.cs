using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSite.Data.Models
{
	public class Booking
	{
		public Guid Id { get; set; }=Guid.NewGuid();
		public string BookingNumber { get; set; } = string.Empty;
		public DateTime BookDate { get; set; }= DateTime.Now;
		public string BookingStatus { get; set; } = String.Empty;

		public Guid TravelDatesId { get; set; }
		public TravelDates? TravelDates { get; set; }

		public DateOnly From { get; set; }=new DateOnly();
		public DateOnly To { get; set; }=new DateOnly();

		public Guid TravelId { get; set; }
		public Travel? Travel { get; set; }

		public string UserId { get; set; } = "";
		public User? User { get; set; }
		public Order? Order { get; set; }
		public List<BookingNotification> BookingNotifications { get; set; } = new List<BookingNotification>();
	}
}
