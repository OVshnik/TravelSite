using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net;
using System.Security.Claims;
using System.Text;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Account;
using TravelSite.Models.Bookings;
using TravelSite.Models.TravelDates;
using TravelSite.Models.Travels;
using TravelSite.Notifications;

namespace TravelSite.Services
{
	public class BookingService : IBookingService
	{
		private readonly IMapper _mapper;
		private readonly IBookingRepository _bookingRepository;
		private readonly ITravelRepository _travelRepository;
		private readonly ITravelDatesRepository _travelDatesRepository;
		private readonly UserManager<User> _userManager;
		private readonly INotificationRepository<BookingNotification> _notificationRepository;
		public BookingService(IMapper mapper,
			IBookingRepository bookingRepository,
			ITravelRepository repository,
			ITravelDatesRepository travelDatesRepository,
			UserManager<User> User,
			INotificationRepository<BookingNotification> notificationRepository)
		{
			_mapper = mapper;
			_bookingRepository = bookingRepository;
			_travelRepository = repository;
			_userManager = User;
			_travelDatesRepository = travelDatesRepository;
			_notificationRepository = notificationRepository;
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
					BookingStatus = "Pending",
					BookingNumber = await GenerateBookingNumber(travel.Name),
				};

				model.Travel.Photo = "/uploads/photo/" + travel.Photo;

				var dates = travel.TravelDates.Where(x => x.AvailablePlaces != 0);

				foreach (var date in dates)
				{
					model.TravelDates.Add(_mapper.Map<TravelDatesViewModel>(date));
				}
				return model;
			}
			throw new Exception($"Тур с id '{id}' не найден, либо пользователь с именем '{claims?.Identity?.Name}' не существует");
		}

		public async Task<Guid> AddBookingAsync(CreateBookingViewModel model)
		{
			var dates = model.TravelDates.FirstOrDefault(x => x.isChecked == true);

			if (dates != null && model.Travel != null)
			{
				model.To = dates.To;
				model.From = dates.From;

				var travel = await _travelRepository.GetTravelByIdAsync(model.Travel.Id);

				var user = await _userManager.FindByIdAsync(model.UserId);

				var d = await _travelDatesRepository.GetTravelDatesByIdAsync(dates.Id);
				if (d != null && user != null && travel != null)
				{
					if (d.AvailablePlaces != 0)
					{
						var booking = _mapper.Map<Booking>(model);

						booking.TravelDates = d;
						booking.Travel = travel;
						booking.User = user;

						await _bookingRepository.CreateBookingAsync(booking);
						d.AvailablePlaces--;
						await _travelDatesRepository.UpdateTravelDatesAsync(d);

						await CreateNotification(booking.Id, user.Id, booking.BookingNumber);
						return booking.Id;

					}
				}
			}
			throw new Exception($"тур с Id '{model.Travel?.Id}' не найден");
		}
		public async Task RemoveBookingAsync(Guid id)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			if (booking != null)
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
				if (trDates != null)
				{
					model.Price = trDates.Price;
				}
				return model;

			}
			throw new Exception($"Бронирование с id '{id}' не найдено");
		}

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

		public async Task<EditBookingViewModel> EditBookingAsync(Guid id)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			if (booking != null)
			{
				return _mapper.Map<EditBookingViewModel>(booking);
			}
			throw new Exception($"Бронирование с id'{id}'не найдено");

		}
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
		public async Task CreateNotification(Guid bookId, string userId, string bookNum)
		{
			var admins = await _userManager.GetUsersInRoleAsync("Admin");

			var ids = admins.Select(x => x.Id).ToList();

			var message = $"Бронирование №{bookNum} создано";

			var booking = await _bookingRepository.GetBookingByIdAsync(bookId);
			if (booking != null)
			{
				foreach (var id in ids)
				{
					var notification = new BookingNotification
					{
						Content = message,
						RecipientId = id,
						SenderId = userId,
						BookingId = booking.Id
					};
					await _notificationRepository.CreateNotificationAsync(notification);
				}
			}
		}
		public async Task ConfirmBooking(Guid id, string senderId, string bookNum)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			if (booking != null)
			{
				booking.BookingStatus = "Confirmed";
				await _bookingRepository.UpdateBookingAsync(booking);
				var notification = new BookingNotification
				{
					Content = $"Бронирование №{bookNum} подтверждено",
					RecipientId = booking.UserId,
					SenderId = senderId,
					BookingId = booking.Id
				};
				await _notificationRepository.CreateNotificationAsync(notification);
			}
			else
				throw new Exception($"Бронирование с id'{id}'не найдено");
		}
		public async Task CancelBooking(Guid id, string senderId, string bookNum)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			if (booking != null)
			{
				booking.BookingStatus = "Canceled";
				await _bookingRepository.UpdateBookingAsync(booking);
				var notification = new BookingNotification
				{
					Content = $"Бронирование №{bookNum} отменено",
					RecipientId = booking.UserId,
					SenderId = senderId,
					BookingId = booking.Id
				};
				await _notificationRepository.CreateNotificationAsync(notification);
			}
			else
				throw new Exception($"Бронирование с id'{id}'не найдено");
		}
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
