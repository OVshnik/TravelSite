using TravelSite.Data.Models;
using TravelSite.Models.Travels;

namespace TravelSite.Extensions
{
	public static class TravelFromModel
	{
		public static Travel Convert(this Travel travel, EditTravelViewModel model)
		{
			if (!string.IsNullOrEmpty(model.Name))
			{
				travel.Name = model.Name;
			}
			if (!string.IsNullOrEmpty(model.Description))
			{
				travel.Description = model.Description;
			}
			if (!string.IsNullOrEmpty(model.Category))
			{
				travel.Category = model.Category;
			}
			if (!string.IsNullOrEmpty(model.Video))
			{
				travel.Video = model.Video;
			}
			if (!string.IsNullOrEmpty(model.Photo))
			{
				travel.Photo = model.Photo;
			}
			return travel;
		}
	}
}
