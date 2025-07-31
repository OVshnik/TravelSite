using TravelSite.Data.Models;

namespace TravelSite.Models.TravelPhoto
{
	public class PhotoViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public Guid TravelId { get; set; }
	}
}
