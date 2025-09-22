using TravelSite.Data.Models;
using TravelSite.Models.TravelPhoto;
using TravelSite.Models.TravelVideo;

namespace TravelSite.Models.Travels
{
	public class TravelViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;
		public string Photo { get; set; } = string.Empty;
		public List<PhotoViewModel> PhotoGallery { get; set; } = new List<PhotoViewModel>();
		public List<VideoViewModel> VideoList { get; set; } = new List<VideoViewModel>();
	}
}
