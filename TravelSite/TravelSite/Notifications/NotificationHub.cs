using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;

namespace TravelSite.Notifications
{
	public class NotificationHub:Hub
	{
		private readonly UserManager<User> _userManager;
		private readonly INotificationRepository<BookingNotification> _notificationRepository;
		private readonly IBookingRepository _bookingRepository;
		public NotificationHub(UserManager<User> userManager, 
			INotificationRepository<BookingNotification> notificationRepository,
			IBookingRepository bookingRepository)
		{
			_userManager = userManager;
			_notificationRepository = notificationRepository;
			_bookingRepository = bookingRepository;
		}
		public async Task BookingConfirmed(string userId, string bookNum)
		{
			await Clients.User(userId).SendAsync("BookingConfirmed",$"Ваше бронирование №{bookNum} подтверждено");
		}
		public async Task BookingCanceled(string userId, string bookNum)
		{
			await Clients.User(userId).SendAsync("BookingCanceled", $"Ваше бронирование №{bookNum} отменено");
		}
		public async Task BookingCreate(string userId, string bookNum, string bookId)
		{
			var admins = await _userManager.GetUsersInRoleAsync("Admin");

			var ids=admins.Select(x => x.Id).ToList();

			var message = $"Бронирование №{bookNum} создано";

			await Clients.Users(ids).SendAsync("BookingCreateNotify", message, userId);
		}
	}
}
