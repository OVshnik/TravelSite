using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Bookings;
using TravelSite.Models.TravelDates;
using TravelSite.Models.Travels;

namespace TravelSite.Services
{
	public class BookingService : IBookingService
	{
		private readonly IMapper _mapper;
		private readonly IBookingRepository _bookingRepository;
		private readonly ITravelRepository _travelRepository;
		private readonly ITravelDatesRepository _travelDatesRepository;
		private readonly UserManager<User> _userManager;
		private readonly INotificationService _notificationService;
		public BookingService(IMapper mapper,
			IBookingRepository bookingRepository,
			ITravelRepository repository,
			ITravelDatesRepository travelDatesRepository,
			UserManager<User> User,
			INotificationService notificationService)
		{
			_mapper = mapper;
			_bookingRepository = bookingRepository;
			_travelRepository = repository;
			_userManager = User;
			_travelDatesRepository = travelDatesRepository;
			_notificationService = notificationService;
		}
		/// <summary>
		/// Метод для создания модели создания бронирования
		/// </summary>
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
					BookingStatus = "Pending",
					BookingNumber = await GenerateBookingNumber(travel.Name),
				};

				model.Travel.Photo = "/uploads/photo/" + travel.Photo;

				var dates = travel.TravelDates.Where(x => x.AvailablePlaces != 0);

				foreach (var date in dates)
				{
					model.TravelDates?.Add(_mapper.Map<TravelDatesViewModel>(date));
				}
				return model;
			}
			throw new Exception($"Тур с id '{id}' не найден, либо пользователь с именем '{claims?.Identity?.Name}' не существует");
		}
		/// <summary>
		/// Метод для добавления бронирования
		/// </summary>
		public async Task<Guid> AddBookingAsync(CreateBookingViewModel model)
		{
			var dates = model.TravelDates?.FirstOrDefault(x => x.isChecked == true);

			if (dates != null && model.Travel != null)
			{
				model.To = dates.To;
				model.From = dates.From;

				var travel = await _travelRepository.GetTravelByIdAsync(model.Travel.Id);

				var user = await _userManager.FindByIdAsync(model.UserId);

				var d = await _travelDatesRepository.GetTravelDatesByIdAsync(dates.Id);

				var bookings = await _bookingRepository.GetAllBookingsAsync();


				if (d != null && user != null && travel != null)
				{
					var checkBooking = await CheckDates(user.Id,travel.Id,d.Id);
					if (checkBooking && d.AvailablePlaces != 0)
					{
						var booking = _mapper.Map<Booking>(model);

						booking.TravelDates = d;
						booking.Travel = travel;
						booking.User = user;

						await _bookingRepository.CreateBookingAsync(booking);
						d.AvailablePlaces--;
						await _travelDatesRepository.UpdateTravelDatesAsync(d);

						return booking.Id;
					}
					else
					{
						throw new Exception($"Тур с этими датами уже забронирован пользователем");
					}
				}
			}
			throw new Exception($"тур с Id '{model.Travel?.Id}' не найден");
		}
		/// <summary>
		/// Метод для проверки дат бронирования
		/// </summary>
		public async Task<bool> CheckDates(string userId, Guid travelId, Guid datesId)
		{
			var bookings = await _bookingRepository.GetAllBookingsAsync();
			if (bookings != null)
			{
				var checkBooking = bookings.Where(x => (x.UserId == userId) &&
												 (x.TravelId == travelId) &&
												 (x.TravelDatesId == datesId)).FirstOrDefault();
				if (checkBooking != null)
				{
					return false;
				}
				return true;
			}
			return true;
		}
		/// <summary>
		/// Метод для удаления бронирования
		/// </summary>
		public async Task RemoveBookingAsync(Guid id)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			if (booking != null)
			{
				await _bookingRepository.DeleteBookingAsync(id);
			}
		}
		/// <summary>
		/// Метод для получения модели бронирования по id
		/// </summary>
		public async Task<BookingViewModel> GetBookingAsync(Guid id)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);

			if (booking != null)
			{
				var model = _mapper.Map<BookingViewModel>(booking);
				var trDates = await _travelDatesRepository.GetTravelDatesByIdAsync(booking.TravelDatesId);
				if (trDates != null)
				{
					model.Price = trDates.Price;
				}
				return model;

			}
			throw new Exception($"Бронирование с id '{id}' не найдено");
		}
		/// <summary>
		/// Метод для получения моделей всех бронирований
		/// </summary>
		public async Task<List<BookingViewModel>> GetAllBookingAsync()
		{
			var bookings = await _bookingRepository.GetAllBookingsAsync();
			var listBookings = new List<BookingViewModel>();
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
		/// <summary>
		/// Метод создающий модель бронирования, для последующего редактирования
		/// </summary>
		public async Task<EditBookingViewModel> EditBookingAsync(Guid id)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			var trDates = await _travelDatesRepository.GetTravelDatesByIdAsync(booking.TravelDatesId);
			if (booking != null && trDates != null)
			{
				var model = _mapper.Map<EditBookingViewModel>(booking);
				model.Price = trDates.Price;
				return model;
			}
			throw new Exception($"Бронирование с id'{id}'не найдено");

		}
		/// <summary>
		/// Метод для обновления данных бронирования
		/// </summary>
		public async Task UpdateBookingAsync(EditBookingViewModel model)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(model.Id);
			if (booking != null)
			{
				booking.BookDate = model.BookDate;
				booking.BookingStatus = model.BookingStatus;
				await _bookingRepository.UpdateBookingAsync(booking);
			}
		}
		/// <summary>
		/// Метод для генерации номера бронирования
		/// </summary>
		public async Task<string> GenerateBookingNumber(string trName)
		{
			if (!string.IsNullOrEmpty(trName))
			{
				var bookings = await _bookingRepository.GetAllBookingsAsync();
				string counter = "0001";
				if (bookings != null)
				{
					var lastBooking = bookings.OrderByDescending(x => x.BookDate).FirstOrDefault();
					counter = (Convert.ToInt32(lastBooking?.BookingNumber.Substring(lastBooking.BookingNumber.Length - 4)) + 1).ToString();
					if (counter.Length <= 3)
					{
						while (counter.Length < 4)
						{
							counter = counter.Insert(0, "0");
						}
					}
				}

				var rndNum = new Random().Next(0, 99999).ToString();

				var bookingNum = trName.ToUpper() + "-" + rndNum + "-" + counter;

				var check = bookings?.Where(x => x.BookingNumber == bookingNum).FirstOrDefault();

				if (check == null)
				{
					return bookingNum;
				}
				return await GenerateBookingNumber(trName);
			}
			throw new NullReferenceException();
		}
	}
}
