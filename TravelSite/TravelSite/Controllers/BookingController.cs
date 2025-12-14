using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelSite.Data.Models;
using TravelSite.Models.Bookings;
using TravelSite.Services;

namespace TravelSite.Controllers
{
	public class BookingController : Controller
	{
		private readonly IBookingService _bookingService;
		private readonly INotificationService _notificationService;
		private readonly UserManager<User> _userManager;
		private readonly ILogger<BookingController> _logger;
		public BookingController(IBookingService bookingService, INotificationService notificationService, UserManager<User> userManager, ILogger<BookingController> logger)
		{
			_bookingService = bookingService;
			_notificationService = notificationService;
			_userManager = userManager;
			_logger = logger;
		}
		/// <summary>
		/// [Post] Метод, для добавления модели бронирования
		/// </summary>
		[HttpPost]
		[Route("PrepareAddBooking")]
		public async Task<IActionResult> PrepareAddBooking(Guid id)
		{
			var user = User;
			if (user != null && User.Identity.IsAuthenticated)
			{
				var model = await _bookingService.AddBookingAsync(id, user);
				return View("AddBooking", model);
			}
			return RedirectToAction("Login", "Account");
		}
		/// <summary>
		/// [Post] Метод, для добавления бронирования
		/// </summary>
		[HttpPost]
		[Route("AddBooking")]
		public async Task<IActionResult> AddBooking(CreateBookingViewModel model)
		{
			if (ModelState.IsValid)
			{
				var bookId = await _bookingService.AddBookingAsync(model);
				var url = Url.Action("GetBooking", "Booking", new { id = bookId },"http");

				await _notificationService.CreateBookingNotification(bookId, model.UserId, model.BookingNumber, url);

				_logger.LogInformation($"Бронирование номер {model.BookingNumber} создано, пользователем с id={model.UserId}", model.BookingNumber);
				return RedirectToAction("PrepareAddOrder", "Order", new { id = bookId });
			}
			return RedirectToAction("PrepareAddBooking", "Booking", new { id = model.Travel?.Id });
		}
		/// <summary>
		/// [Get] Метод, для получения бронирования по id
		/// </summary>
		[HttpGet]
		[Route("GetBooking")]
		public async Task<IActionResult> GetBooking(Guid id)
		{
			var model = await _bookingService.GetBookingAsync(id);
			return View("BookingPage", model);
		}
		/// <summary>
		/// [Get] Метод, для получения всех бронирований
		/// </summary>
		[HttpGet]
		[Route("GetAllBooking")]
		public async Task<IActionResult> GetAllBooking()
		{
			var bookings = await _bookingService.GetAllBookingAsync();
			return View("BookingList", bookings);
		}
		/// <summary>
		/// [Get] Метод, для получения последнего бронирования по id пользователя
		/// </summary>
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetLastBookingByUser(string id)
		{
			var bookings = await _bookingService.GetAllBookingAsync();
			var lastBooking = bookings.Where(x => x.User?.Id == id).OrderByDescending(x => x.BookDate).FirstOrDefault();
			return Json(lastBooking);
		}
		/// <summary>
		/// [Get] Метод, для редактирования бронирования 
		/// </summary>
		[Authorize("Admin")]
		[HttpGet]
		[Route("EditBooking")]
		public async Task<IActionResult> EditBooking(Guid id)
		{
			var model = await _bookingService.EditBookingAsync(id);
			return View("EditBooking", model);
		}
		/// <summary>
		/// [Post] Метод, для обновления бронирования
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("UpdateBooking")]
		public async Task<IActionResult> UpdateBooking(EditBookingViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _bookingService.UpdateBookingAsync(model);
				_logger.LogInformation($"Бронирование номер {model.BookingNumber} изменено", model.BookingNumber);
				return RedirectToAction("GetAllBooking");
			}
			return RedirectToAction("EditBooking");
		}
		/// <summary>
		/// [Post] Метод, для удаления бронирования 
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("DeleteBooking")]
		public async Task<IActionResult> DeleteBooking(Guid id)
		{
			var booking=await _bookingService.GetBookingAsync(id);
			await _bookingService.RemoveBookingAsync(id);
			_logger.LogInformation($"Бронирование номер {booking.BookingNumber} удалено", booking.BookingNumber);
			return RedirectToAction("GetAllBooking");
		}
		/// <summary>
		/// [Get] Метод, для проверки статуса бронирования
		/// </summary>
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> CheckBooking(Guid id)
		{
			var booking = await _bookingService.GetBookingAsync(id);
			return Json(booking.BookingStatus);
		}
		/// <summary>
		/// [Get] Метод, для проверки свободных дат тура
		/// </summary>
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> CheckDates(string userId, Guid travelId, Guid datesId)
		{
			return Json(await _bookingService.CheckDates(userId, travelId, datesId));
		}
		/// <summary>
		/// [Post] Метод, для подтверждения бронирования
		/// </summary>
		[HttpPost]
		[Authorize("Admin")]
		public async Task<IActionResult> ConfirmBooking(Guid id, string senderId, string bookNum)
		{
			var url = Url.ActionLink("GetBooking", "Booking") + "?=" + id.ToString();
			await _notificationService.ConfirmBookingNotification(id, senderId, bookNum, url);
			_logger.LogInformation($"Статус бронирования {bookNum} изменен на 'Confirmed'", bookNum);
			return Ok();
		}
		/// <summary>
		/// [Post] Метод, для отмены бронирования
		/// </summary>
		[HttpPost]
		[Authorize("Admin")]
		public async Task<IActionResult> CancelBooking(Guid id, string senderId, string bookNum)
		{
			var url = Url.ActionLink("GetBooking", "Booking") + "?=" + id.ToString();
			await _notificationService.CancelBookingNotification(id, senderId, bookNum, url);
			_logger.LogInformation($"Статус бронирования {bookNum} изменен на 'Canceled'", bookNum);
			return Ok();
		}
	}
}
