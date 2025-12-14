namespace TravelSite.Data.Models
{
	public class Travel
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;
		public string Photo { get; set; } = string.Empty;
		public List<TravelDates> TravelDates { get; set; }=new List<TravelDates>();
		public List<Booking> BookingList { get; set; } = new List<Booking>();
		public List<User> UserList { get; set; } = new List<User>();
		public List<TravelPhoto> PhotoGallery { get; set; } = new List<TravelPhoto>();
		public List<TravelVideo> VideoList { get; set; } = new List<TravelVideo>();

	}
}
