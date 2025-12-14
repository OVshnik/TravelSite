using System.ComponentModel.DataAnnotations.Schema;
namespace TravelSite.Data.Models
{
	[Table("BookingNotification")]
	public class BookingNotification:Notification
	{
		public Guid BookingId { get; set; } 
		public Booking? Booking { get; set; }
	}
}
