using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Account;
using TravelSite.Models.Bookings;
using TravelSite.Models.TravelDates;
using TravelSite.Models.Travels;

namespace TravelSite.Services
{
	public class BookingService:IBookingService
	{
		private readonly IMapper _mapper;
		private readonly IBookingRepository _bookingRepository;
		private readonly ITravelRepository _travelRepository;
		private readonly ITravelDatesRepository _travelDatesRepository;
		private readonly UserManager<User> _userManager;
		public BookingService(IMapper mapper, IBookingRepository bookingRepository, ITravelRepository repository,ITravelDatesRepository travelDatesRepository, UserManager<User> User)
		{
			_mapper = mapper;
			_bookingRepository = bookingRepository;
			_travelRepository = repository;
			_userManager = User;
			_travelDatesRepository = travelDatesRepository;
		}
		public async Task<CreateBookingViewModel> AddBookingAsync(Guid id, ClaimsPrincipal claims)
		{
			var travel = await _travelRepository.GetTravelByIdAsync(id);

			var user = await _userManager.GetUserAsync(claims);

			if (user != null && travel != null)
			{
				var model = new CreateBookingViewModel()
				{
					UserId = user.Id,
					Travel = _mapper.Map<TravelViewModel>(travel),
					BookDate = DateTime.Now,
					BookingStatus = "Booked",
				};
				foreach (var date in travel.TravelDates)
				{
					model.TravelDates.Add(_mapper.Map<TravelDatesViewModel>(date));
				}
				return model;
			}
			throw new Exception($"Тур с id '{id}' не найден, либо пользователь с именем '{claims?.Identity?.Name}' не существует");
		}

		public async Task<Guid> AddBookingAsync(CreateBookingViewModel model)
		{
			var dates = model.TravelDates.FirstOrDefault(x=>x.isChecked==true);

			if (dates != null&&model.Travel!=null)
			{
				model.To = dates.To;
				model.From=dates.From;

				var travel = await _travelRepository.GetTravelByIdAsync(model.Travel.Id);

				var user = await _userManager.FindByIdAsync(model.UserId);

				var d = await _travelDatesRepository.GetTravelDatesByIdAsync(dates.Id);
				if (d != null && user != null&&travel!=null)
				{
					if (d.AvailablePlaces != 0)
					{
						var booking = _mapper.Map<Booking>(model);

						//booking.ClientId=user.Id;
						//booking.TravelId=travel.Id;
						//booking.TravelDatesId=d.Id;

						booking.TravelDates = d;
						booking.Travel=travel;
						booking.User=user;

						await _bookingRepository.CreateBookingAsync(booking);
						d.AvailablePlaces--;
						await _travelDatesRepository.UpdateTravelDatesAsync(d);
						return booking.Id;
					}
				}
			}
			throw new Exception($"тур с Id '{model.Travel?.Id}' не найден");
		}
		public async Task RemoveBookingAsync(Guid id)
		{
			var booking=await _bookingRepository.GetBookingByIdAsync(id);
			if(booking != null)
			{
				await _bookingRepository.DeleteBookingAsync(id);
			}
		}

		public async Task<BookingViewModel> GetBookingAsync(Guid id)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			
			if (booking != null)
			{
				var model = _mapper.Map<BookingViewModel>(booking);
				var trDates = await _travelDatesRepository.GetTravelDatesByIdAsync(booking.TravelDatesId);
				if(trDates != null)
				{
					model.Price=trDates.Price;
				}
				return model;
				
			}
			throw new Exception($"Бронирование с id '{id}' не найдено");
		}

		public async Task<List<BookingViewModel>> GetAllBookingAsync()
		{
			var bookings=await _bookingRepository.GetAllBookingsAsync();
			var listBookings=new List<BookingViewModel>();
			if (bookings != null)
			{
				foreach (var booking in bookings)
				{
					listBookings.Add(_mapper.Map<BookingViewModel>(booking));
				}
				return listBookings;
			}
			return listBookings;
		}

		public async Task<EditBookingViewModel> EditBookingAsync(Guid id)
		{
			var booking=await _bookingRepository.GetBookingByIdAsync(id);
			if( booking != null )
			{
				return _mapper.Map<EditBookingViewModel>(booking);
			}
			throw new Exception($"Бронирование с id'{id}'не найдено");

		}

		public async Task UpdateBookingAsync(EditBookingViewModel model)
		{
			var booking=await _bookingRepository.GetBookingByIdAsync(model.Id);
			if( booking != null )
			{
				booking.BookDate=model.BookDate;
				booking.BookingStatus=model.BookingStatus;
				await _bookingRepository.UpdateBookingAsync(booking);
			}
		}

	}
}
