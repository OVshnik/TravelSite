using TravelSite.Data.Models;
using TravelSite.Models.Travels;

namespace TravelSite.Models.TravelDates
{
	public class TravelDatesViewModel
	{
		public Guid Id { get; set; }
		public DateOnly From { get; set; }
		public DateOnly To { get; set; }
		public int DaysCount { get; set; }
		public int MaxPlaces { get; set; }
		public int AvailablePlaces { get; set; }
		public int Price { get; set; }
		public bool isChecked { get; set; }
		public Guid TravelId { get; set; }
		public TravelViewModel? Travel { get; set; }
	}
}
