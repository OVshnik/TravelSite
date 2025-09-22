using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSite.Data.Models
{
	[Table("BookingNotification")]
	public class BookingNotification:Notification
	{
		public Guid BookingId { get; set; } 
		public Booking? Booking { get; set; }
	}
}
