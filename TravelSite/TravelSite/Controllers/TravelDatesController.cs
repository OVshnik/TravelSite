using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelSite.Data.Repository;
using TravelSite.Models.TravelDates;
using TravelSite.Services;

namespace TravelSite.Controllers
{
	public class TravelDatesController : Controller
	{
		private readonly ITravelDatesService _travelDatesService;
		public TravelDatesController(ITravelDatesService travelDatesService)
		{
			_travelDatesService = travelDatesService;
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("AddTravelDates")]
		public async Task<IActionResult> AddTravelDates(Guid id)
		{
			var model = await _travelDatesService.AddTravelDates(id);
			return View(model);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("AddTravelDatesInDB")]
		public async Task<IActionResult> AddTravelDatesInDB(CreateTravelDatesViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _travelDatesService.AddTravelDates(model);
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("AddTravelDates");
		}
		[HttpGet]
		[Route("GetTravelDates")]
		public async Task<IActionResult> GetTravelDates(Guid id)
		{
			var model = await _travelDatesService.GetTravelDatesById(id);
			return View("TravelDatesPage",model);
		}
		[Authorize("Admin")]
		[HttpGet]
		[Route("GetAllTravelDates")]
		public async Task<IActionResult> GetAllTravelDates()
		{
			var model = await _travelDatesService.GetAllTravelDates();
			return View("TravelDatesList",model);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("EditTravelDates")]
		public async Task<IActionResult> EditTravelDates(Guid id)
		{
			var model = await _travelDatesService.EditTravelDates(id);
			return View(model);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("UpdateTravelDates")]
		public async Task<IActionResult> UpdateTravelDates(EditTravelDatesViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _travelDatesService.UpdateTravelDates(model);
				return RedirectToAction("GetAllTravelDates");
			}
			return RedirectToAction("EditTravelDates");
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("DeleteTravelDates")]
		public async Task<IActionResult> DeleteTravelDates(Guid id)
		{
			await _travelDatesService.RemoveTravelDates(id);
			return RedirectToAction("GetAllTravelDates");
		}
	}
}
