using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
		[ForeignKey("SenderId")]
		public string? SenderId { get; set; }
		public User? Sender { get; set; }
		[ForeignKey("RecipientId")]
		public string? RecipientId { get; set; }
		public User? Recipient { get; set; }
	}
}
