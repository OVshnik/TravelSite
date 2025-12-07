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
		private readonly ILogger<NotificationController> _logger;
		public NotificationController(INotificationService notificationService,UserManager<User> userManager, ILogger<NotificationController> logger)
		{
			_notificationService = notificationService;
			_userManager = userManager;
			_logger = logger;
		}
		/// <summary>
		/// [Get] Метод, для получения всех уведомлений
		/// </summary>
		[Authorize]
		[Route("GetNotifications")]
		public async Task<IActionResult> GetNotifications()
		{
			var nots = await _notificationService.GetAllNotificationAsync();
			return View("NotificationList", nots);
		}
		/// <summary>
		/// [Get] Метод, для удаления всех уведомлений конкретного пользователя
		/// </summary>
		[Authorize]
		[Route("ClearNotifications")]
		public async Task<IActionResult> ClearNotifications(string userId)
		{
			var user=await _userManager.FindByIdAsync(userId);
			await _notificationService.RemoveAllNotificationByUserAsync(userId);
			_logger.LogInformation($"Уведомления пользователя с логином {user?.Email} очищены");
			return RedirectToAction("GetNotifications");
		}
		/// <summary>
		/// [Post] Метод, для отметки всех уведомлений как прочитанных
		/// </summary>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> MarkNotificationAsDelivered(Guid id)
		{
			await _notificationService.MarkNotificationAsDeliveredAsync(id);
			return Ok();
		}
		/// <summary>
		/// [Get] Метод, для получения всех уведомлений
		/// </summary>
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
