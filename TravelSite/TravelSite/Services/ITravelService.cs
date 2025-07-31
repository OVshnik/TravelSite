using TravelSite.Models.Travels;

namespace TravelSite.Services
{
	public interface ITravelService
	{
		public Task AddTravelAsync(CreateTravelViewModel model);
		public Task RemoveTravelAsync(Guid id);
		public Task <EditTravelViewModel>EditTravelAsync(Guid id);
		public Task UpdateTravelAsync(EditTravelViewModel model);
		public Task <TravelViewModel> GetTravelAsync(Guid id);
		public Task<List<TravelViewModel>> GetAllTravelAsync();
		public Task<List<TravelViewModel>> CreateSearch(string search);
	}
}
