using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSite.Data.Models
{
	public class Notification
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Content { get; set; } = String.Empty;
		public DateTime Created { get; set; }= DateTime.Now;
		public bool Delivered { get; set; }= false;
		public string SenderId { get; set; } = String.Empty;
		public User? Sender { get; set; }
		public string RecipientId { get; set; } = String.Empty;
		public User? Recipient { get; set; }
	}
}
