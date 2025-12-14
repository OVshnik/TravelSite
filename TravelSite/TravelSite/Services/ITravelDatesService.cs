using TravelSite.Models.TravelDates;

namespace TravelSite.Services
{
	public interface ITravelDatesService
	{
		public Task AddTravelDates(CreateTravelDatesViewModel model);
		public Task<CreateTravelDatesViewModel> AddTravelDates(Guid id);
		public Task RemoveTravelDates(Guid id);
		public Task<TravelDatesViewModel> GetTravelDatesById(Guid id);
		public Task<List<TravelDatesViewModel>> GetAllTravelDates();
		public Task<EditTravelDatesViewModel> EditTravelDates(Guid id);
		public Task UpdateTravelDates(EditTravelDatesViewModel model);
	}
}
