using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelSite.Models.Travels;
using TravelSite.Services;

namespace TravelSite.Controllers
{
	public class TravelController : Controller
	{
		private readonly ITravelService _travelService;
		private readonly ILogger<TravelController> _logger;
		public TravelController(ITravelService travelService, ILogger<TravelController> logger)
		{
			_travelService = travelService;
			_logger = logger;
		}
		/// <summary>
		/// [Get] Метод, для добавления тура
		/// </summary>
		[Authorize("Admin")]
		[HttpGet]
		[Route("AddTravel")]
		public IActionResult AddTravel()
		{
			var model = new CreateTravelViewModel();
			return View(model);
		}
		/// <summary>
		/// [Post] Метод, для добавления тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("AddTravel")]
		public async Task<IActionResult> AddTravel(CreateTravelViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _travelService.AddTravelAsync(model);
				_logger.LogInformation($"Тур с id={model.Id} добавлен",model.Id);
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("AddTravel");
		}
		/// <summary>
		/// [Get] Метод, для получения тура
		/// </summary>
		[HttpGet]
		[Route("GetTravel")]
		public async Task<IActionResult> GetTravel(Guid id)
		{
			var model = await _travelService.GetTravelAsync(id);
			return View("TravelPage", model);
		}
		/// <summary>
		/// [Get] Метод, для получения всех туров
		/// </summary>
		[HttpGet]
		[Route("GetAllTravels")]
		public async Task<IActionResult> GetAllTravels()
		{
			var travels = await _travelService.GetAllTravelAsync();
			return View("TravelList", travels);
		}
		/// <summary>
		/// [Post] Метод, для редактирования тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("EditTravel")]
		public async Task<IActionResult> EditTravel(Guid id)
		{
			var model = await _travelService.EditTravelAsync(id);
			return View(model);
		}
		/// <summary>
		/// [Post] Метод, для обновления данных тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("UpdateTravel")]
		public async Task<IActionResult> UpdateTravel(EditTravelViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _travelService.UpdateTravelAsync(model);
				_logger.LogInformation($"Тур с id={model.Id} изменен",model.Id);
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("EditTravel", "Travel", model.Id);
		}
		/// <summary>
		/// [Post] Метод, для удаления тура
		/// </summary>
		[Authorize("Admin")]
		[HttpPost]
		[Route("DeleteTravel")]
		public async Task<IActionResult> DeleteTravel(Guid id)
		{
			var travel=await _travelService.GetTravelAsync(id);
			await _travelService.RemoveTravelAsync(id);
			_logger.LogInformation($"Тур с id={travel.Id} удален",travel.Id);
			return RedirectToAction("GetAllTravels");
		}
		/// <summary>
		/// [Post] Метод, для поиска тура
		/// </summary>
		[Route("SearchTravel")]
		[HttpPost]
		public async Task<IActionResult> SearchTravel(string search)
		{
			var model = await _travelService.CreateSearch(search);

			return View("SearchList", model);
		}

	}
}
