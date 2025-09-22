using Microsoft.AspNetCore.Mvc;
using TravelSite.Services;
using Microsoft.AspNetCore.Authorization;

namespace TravelSite.Controllers
{
	public class NotificationController : Controller
	{
		private readonly INotificationService _notificationService;
		public NotificationController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}
		[Authorize]
		[Route("GetNotifications")]
		public async Task<IActionResult> GetNotifications()
		{
			var nots = await _notificationService.GetAllNotificationAsync();
			return View("NotificationList", nots);
		}
		[Authorize]
		[Route("ClearNotifications")]
		public async Task<IActionResult> ClearNotifications(string userId)
		{
			await _notificationService.RemoveAllNotificationByUserAsync(userId);
			return RedirectToAction("GetNotifications");
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> MarkNotificationAsDelivered(Guid id)
		{
			await _notificationService.MarkNotificationAsDeliveredAsync(id);
			return Ok();
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllNewNotifications()
		{
			var nots = await _notificationService.GetAllNotificationAsync();
			nots=nots.Where(x=>x.Delivered==false).ToList();
			return Json(nots);
		}
	}
}
