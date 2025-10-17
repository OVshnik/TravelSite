using Microsoft.AspNetCore.Mvc;
using TravelSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TravelSite.Data.Models;

namespace TravelSite.Controllers
{
	public class NotificationController : Controller
	{
		private readonly INotificationService _notificationService;
		private readonly UserManager<User> _userManager;
		public NotificationController(INotificationService notificationService,UserManager<User> userManager)
		{
			_notificationService = notificationService;
			_userManager = userManager;
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
			var currentUser=await _userManager.GetUserAsync(User);
			if(currentUser != null)
			{
				var nots = await _notificationService.GetAllNotificationAsync();
				nots = nots.Where(x => x.Delivered == false).Where(z => z.RecipientId == currentUser.Id).ToList();
				return Json(nots);
			}
			throw new Exception("Пользователь не авторизован");
		}
	}
}
