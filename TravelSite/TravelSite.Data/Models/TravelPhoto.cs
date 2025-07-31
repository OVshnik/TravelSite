using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSite.Data.Models
{
	public class TravelPhoto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }=String.Empty;

		public Guid TravelId { get; set; }
		public Travel ?Travel { get; set; }

	}
}
