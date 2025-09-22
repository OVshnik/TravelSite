namespace TravelSite.Models.TravelVideo
{
	public class VideoViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public Guid TravelId { get; set; }
		public bool IsChecked { get; set; }
	}
}
