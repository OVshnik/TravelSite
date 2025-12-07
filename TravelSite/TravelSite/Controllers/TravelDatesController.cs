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
		private readonly ILogger<TravelDatesController> _logger;
		public TravelDatesController(ITravelDatesService travelDatesService, ILogger<TravelDatesController> logger)
		{
			_travelDatesService = travelDatesService;
			_logger = logger;
		}
		/// <summary>
		/// [Post] Метод, для добавления дат тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("AddTravelDates")]
		public async Task<IActionResult> AddTravelDates(Guid id)
		{
			var model = await _travelDatesService.AddTravelDates(id);
			return View(model);
		}
		/// <summary>
		/// [Post] Метод, для добавления дат тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("AddTravelDatesInDB")]
		public async Task<IActionResult> AddTravelDatesInDB(CreateTravelDatesViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _travelDatesService.AddTravelDates(model);
				_logger.LogInformation($"Добавлены даты для тура с id={model.Travel.Id}",model.Id);
				return RedirectToAction("Index", "Home");
			}
			return View("AddTravelDates",model);
		}
		/// <summary>
		/// [Get] Метод, для получения определенных дат тура
		/// </summary>
		[HttpGet]
		[Route("GetTravelDates")]
		public async Task<IActionResult> GetTravelDates(Guid id)
		{
			var model = await _travelDatesService.GetTravelDatesById(id);
			return View("TravelDatesPage",model);
		}
		/// <summary>
		/// [Get] Метод, для получения всех дат тура
		/// </summary>
		[Authorize("Admin")]
		[HttpGet]
		[Route("GetAllTravelDates")]
		public async Task<IActionResult> GetAllTravelDates()
		{
			var model = await _travelDatesService.GetAllTravelDates();
			return View("TravelDatesList",model);
		}
		/// <summary>
		/// [Post] Метод, для редактирования дат тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("EditTravelDates")]
		public async Task<IActionResult> EditTravelDates(Guid id)
		{
			var model = await _travelDatesService.EditTravelDates(id);
			return View(model);
		}
		/// <summary>
		/// [Post] Метод, для обновления дат тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("UpdateTravelDates")]
		public async Task<IActionResult> UpdateTravelDates(EditTravelDatesViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _travelDatesService.UpdateTravelDates(model);
				_logger.LogInformation($"Изменены даты для тура с id={model?.Travel?.Id}", model?.Id);
				return RedirectToAction("GetAllTravelDates");
			}
			return RedirectToAction("EditTravelDates");
		}
		/// <summary>
		/// [Post] Метод, для удаления дат тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("DeleteTravelDates")]
		public async Task<IActionResult> DeleteTravelDates(Guid id)
		{
			var trDates=await _travelDatesService.GetTravelDatesById(id);
			await _travelDatesService.RemoveTravelDates(id);
			_logger.LogInformation($"Удалены даты для тура с id={trDates?.Travel?.Id}", trDates?.Id);
			return RedirectToAction("GetAllTravelDates");
		}
	}
}
