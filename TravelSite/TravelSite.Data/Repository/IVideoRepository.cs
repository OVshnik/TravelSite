using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public interface IVideoRepository
	{
		Task CreateVideoAsync(TravelVideo video);
		Task<List<TravelVideo>> GetAllVideoAsync();
		Task<TravelVideo?> GetVideoByIdAsync(Guid id);
		Task UpdateVideoAsync(TravelVideo video);
		Task DeleteVideoAsync(Guid id);
	}
}
