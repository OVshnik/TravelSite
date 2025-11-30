using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelSite.Data.Models;
using TravelSite.Models.Bookings;
using TravelSite.Models.TravelDates;
using TravelSite.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TravelSite.Controllers
{
	public class BookingController : Controller
	{
		private readonly IBookingService _bookingService;
		private readonly INotificationService _notificationService;
		private readonly UserManager<User> _userManager;
		public BookingController(IBookingService bookingService, INotificationService notificationService, UserManager<User> userManager)
		{
			_bookingService = bookingService;
			_notificationService = notificationService;
			_userManager = userManager;
		}
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
		[HttpPost]
		[Route("AddBooking")]
		public async Task<IActionResult> AddBooking(CreateBookingViewModel model)
		{
			if (ModelState.IsValid)
			{
				var bookId = await _bookingService.AddBookingAsync(model);
				var url = Url.RouteUrl("GetBooking") + "?=" + bookId.ToString();
				await _notificationService.CreateBookingNotification(bookId, model.UserId, model.BookingNumber, url);
				return RedirectToAction("PrepareAddOrder", "Order", new { id = bookId });
			}
			return RedirectToAction("PrepareAddBooking", "Booking", new { id = model.Travel?.Id });
		}
		[HttpGet]
		[Route("GetBooking")]
		public async Task<IActionResult> GetBooking(Guid id)
		{
			var model = await _bookingService.GetBookingAsync(id);
			return View("BookingPage", model);
		}
		[HttpGet]
		[Route("GetAllBooking")]
		public async Task<IActionResult> GetAllBooking()
		{
			var bookings = await _bookingService.GetAllBookingAsync();
			return View("BookingList", bookings);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetLastBookingByUser(string id)
		{
			var bookings = await _bookingService.GetAllBookingAsync();
			var lastBooking = bookings.Where(x => x.User?.Id == id).OrderByDescending(x => x.BookDate).FirstOrDefault();
			return Json(lastBooking);
		}

		[Authorize("Admin")]
		[HttpGet]
		[Route("EditBooking")]
		public async Task<IActionResult> EditBooking(Guid id)
		{
			var model = await _bookingService.EditBookingAsync(id);
			return View("EditBooking", model);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("UpdateBooking")]
		public async Task<IActionResult> UpdateBooking(EditBookingViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _bookingService.UpdateBookingAsync(model);
				return RedirectToAction("GetAllBooking");
			}
			return RedirectToAction("EditBooking");
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("DeleteBooking")]
		public async Task<IActionResult> DeleteBooking(Guid id)
		{
			await _bookingService.RemoveBookingAsync(id);
			return RedirectToAction("GetAllBooking");
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> CheckBooking(Guid id)
		{
			var booking = await _bookingService.GetBookingAsync(id);
			return Json(booking.BookingStatus);
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> CheckDates(string userId, Guid travelId, Guid datesId)
		{
			return Json(await _bookingService.CheckDates(userId, travelId, datesId));
		}
		[HttpPost]
		[Authorize("Admin")]
		public async Task<IActionResult> ConfirmBooking(Guid id, string senderId, string bookNum)
		{
			var url = Url.ActionLink("GetBooking", "Booking") + "?=" + id.ToString();
			await _notificationService.ConfirmBookingNotification(id, senderId, bookNum, url);
			return Ok();
		}
		[HttpPost]
		[Authorize("Admin")]
		public async Task<IActionResult> CancelBooking(Guid id, string senderId, string bookNum)
		{
			var url = Url.ActionLink("GetBooking", "Booking") + "?=" + id.ToString();
			await _notificationService.CancelBookingNotification(id, senderId, bookNum, url);
			return Ok();
		}
	}
}
