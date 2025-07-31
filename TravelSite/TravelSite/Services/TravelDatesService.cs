using AutoMapper;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.TravelDates;
using TravelSite.Models.Travels;

namespace TravelSite.Services
{
	public class TravelDatesService : ITravelDatesService
	{
		private readonly IMapper _mapper;
		private readonly ITravelDatesRepository _travelDatesRepository;
		private readonly ITravelRepository _travelRepository;
		public TravelDatesService(IMapper mapper, ITravelDatesRepository travelDatesRepository, ITravelRepository travelRepository)
		{
			_mapper = mapper;
			_travelDatesRepository = travelDatesRepository;
			_travelRepository = travelRepository;
		}
		public async Task<CreateTravelDatesViewModel> AddTravelDates(Guid id)
		{
			var travel = await _travelRepository.GetTravelByIdAsync(id);

			var model = new CreateTravelDatesViewModel()
			{
				Travel = _mapper.Map<TravelViewModel>(travel)
			};
			return model;
		}
		public async Task AddTravelDates(CreateTravelDatesViewModel model)
		{
			model.DaysCount = CalculateAmountDays(model.To, model.From);

			var travelDates = _mapper.Map<TravelDates>(model);

			var travel = await _travelRepository.GetTravelByIdAsync(model.Travel.Id);

			if (travel != null)
			{
				travelDates.TravelId = travel.Id;

				await _travelDatesRepository.CreateTravelDatesAsync(travelDates);

				travel.TravelDates.Add(travelDates);

				await _travelRepository.UpdateTravelAsync(travel);
			}
		}

		public async Task<EditTravelDatesViewModel> EditTravelDates(Guid id)
		{
			var travelDates = await _travelDatesRepository.GetTravelDatesByIdAsync(id);

			if (travelDates != null)
			{
				var model = _mapper.Map<EditTravelDatesViewModel>(travelDates);
				return model;
			}

			throw new Exception($"Даты с id '{id}' не найдены в БД");
		}

		public async Task UpdateTravelDates(EditTravelDatesViewModel model)
		{
			var travelDates = await _travelDatesRepository.GetTravelDatesByIdAsync(model.Id);

			if (travelDates != null)
			{
				travelDates.From = model.From;
				travelDates.To = model.To;
				travelDates.DaysCount = model.DaysCount;
				await _travelDatesRepository.UpdateTravelDatesAsync(travelDates);
			}
		}
		public async Task<List<TravelDatesViewModel>> GetAllTravelDates()
		{
			var travelDatesList = new List<TravelDatesViewModel>();
			var travelDates = await _travelDatesRepository.GetAllTravelDatesAsync();

			if (travelDates != null)
			{
				foreach (var date in travelDates)
				{
					var model = _mapper.Map<TravelDatesViewModel>(date);
					model.Travel = _mapper.Map<TravelViewModel>(await _travelRepository.GetTravelByIdAsync(model.TravelId));
					travelDatesList.Add(model);
				}
				return travelDatesList;
			}
			return travelDatesList;
		}

		public async Task<TravelDatesViewModel> GetTravelDatesById(Guid id)
		{
			var travelDates = await _travelDatesRepository.GetTravelDatesByIdAsync(id);

			if (travelDates != null)
			{
				var model = _mapper.Map<TravelDatesViewModel>(travelDates);
				model.Travel = _mapper.Map<TravelViewModel>(await _travelRepository.GetTravelByIdAsync(travelDates.TravelId));
				return model;
			}
			throw new Exception($"Даты с id '{id}' не найдены в БД");
		}

		public async Task RemoveTravelDates(Guid id)
		{
			var travelDates = await _travelDatesRepository.GetTravelDatesByIdAsync(id);
			if (travelDates != null)
			{
				await _travelDatesRepository.DeleteTravelDatesAsync(id);
			}
		}
		private int CalculateAmountDays(DateOnly to, DateOnly from)
		{
			return Math.Abs(from.DayNumber - to.DayNumber);
		}

	}
}
