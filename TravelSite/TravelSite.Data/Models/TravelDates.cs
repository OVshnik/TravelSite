using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSite.Data.Models
{
	public class TravelDates
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public DateOnly From { get; set; } = new DateOnly();
		public DateOnly To { get; set; }= new DateOnly();
		public int DaysCount { get; set; }
		public int MaxPlaces { get; set; }
		public int AvailablePlaces { get; set; }	
		public int Price { get; set; }
		public Guid TravelId { get; set; }
		public Travel? Travel { get; set; }
		public Booking? Booking { get; set; }

	}
}
