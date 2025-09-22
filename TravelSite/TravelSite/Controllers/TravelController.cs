using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Travels;
using TravelSite.Services;

namespace TravelSite.Controllers
{
	public class TravelController : Controller
	{
		private readonly ITravelService _travelService;
		public TravelController(ITravelService travelService)
		{
			_travelService = travelService;
		}
		[Authorize("Admin")]
		[HttpGet]
		[Route("AddTravel")]
		public IActionResult AddTravel()
		{
			var model = new CreateTravelViewModel();
			return View(model);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("AddTravel")]
		public async Task<IActionResult> AddTravel(CreateTravelViewModel model)
		{
			if (ModelState.IsValid)
			{

				await _travelService.AddTravelAsync(model);
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("AddTravel");
		}
		[HttpGet]
		[Route("GetTravel")]
		public async Task<IActionResult> GetTravel(Guid id)
		{
			var model = await _travelService.GetTravelAsync(id);
			return View("TravelPage", model);
		}
		[HttpGet]
		[Route("GetAllTravels")]
		public async Task<IActionResult> GetAllTravels()
		{
			var travels = await _travelService.GetAllTravelAsync();
			return View("TravelList", travels);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("EditTravel")]
		public async Task<IActionResult> EditTravel(Guid id)
		{
			var model = await _travelService.EditTravelAsync(id);
			return View(model);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("UpdateTravel")]
		public async Task<IActionResult> UpdateTravel(EditTravelViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _travelService.UpdateTravelAsync(model);
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("EditTravel", "Travel", model.Id);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("DeleteTravel")]
		public async Task<IActionResult> DeleteTravel(Guid id)
		{
			await _travelService.RemoveTravelAsync(id);
			return RedirectToAction("GetAllTravels");
		}
		[Route("SearchTravel")]
		[HttpPost]
		public async Task<IActionResult> SearchTravel(string search)
		{
			var model = await _travelService.CreateSearch(search);

			return View("SearchList", model);
		}

	}
}
