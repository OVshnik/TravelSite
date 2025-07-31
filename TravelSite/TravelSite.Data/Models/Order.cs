using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSite.Data.Models
{
	public class Order
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string OrderNumber { get; set; } = string.Empty;
		public string Description { get; set; }=string.Empty;
		public DateTime CreateDate { get; set; }=DateTime.Now;
		public string Status { get; set; } = "Unconfirmed";
		public Guid BookingId {  get; set; }
		public Booking ?Booking { get; set; }
	}
}
