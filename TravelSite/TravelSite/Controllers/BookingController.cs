using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelSite.Data.Repository;
using TravelSite.Models.Bookings;
using TravelSite.Services;

namespace TravelSite.Controllers
{
	public class BookingController : Controller
	{
		private readonly IBookingService _bookingService;
		public BookingController(IBookingService bookingService)
		{
			_bookingService = bookingService;
		}
		[HttpPost]
		[Route("PrepareAddBooking")]
		public async Task<IActionResult> PrepareAddBooking(Guid id)
		{
			var user = User;
			if (user != null)
			{
				var model = await _bookingService.AddBookingAsync(id, user);
				return View("AddBooking", model);
			}
			return RedirectToAction("Index","Home");
		}
		[HttpPost]
		[Route("AddBooking")]
		public async Task<IActionResult> AddBooking(CreateBookingViewModel model)
		{
			if (ModelState.IsValid)
			{
				var bookId=await _bookingService.AddBookingAsync(model);
				return RedirectToAction("PrepareAddOrder", "Order",new {id= bookId});
			}
			return RedirectToAction("PrepareAddBooking", model.Travel?.Id);
		}
		[HttpGet]
		[Route("GetBooking")]
		public async Task<IActionResult> GetBooking(Guid id)
		{
			var model = await _bookingService.GetBookingAsync(id);
			return View("BookingPage",model);
		}
		[HttpGet]
		[Route("GetAllBooking")]
		public async Task<IActionResult> GetAllBooking()
		{
			var bookings = await _bookingService.GetAllBookingAsync();
			return View("BookingList",bookings);
		}
		[Authorize("Admin")]
		[HttpGet]
		[Route("EditBooking")]
		public async Task<IActionResult> EditBooking(Guid id)
		{
			var model = await _bookingService.EditBookingAsync(id);
			return View("EditBooking",model);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("UpdateBooking")]
		public async Task<IActionResult> UpdateBooking(EditBookingViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _bookingService.UpdateBookingAsync(model);
				return RedirectToAction("Index");
			}
			return RedirectToAction("EditBooking");
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("DeleteBooking")]
		public async Task<IActionResult>DeleteBooking(Guid id)
		{
			await _bookingService.RemoveBookingAsync(id);
			return RedirectToAction("GetAllBooking");
		}
	}
}
