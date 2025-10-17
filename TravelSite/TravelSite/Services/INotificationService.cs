using System.Security.Claims;
using TravelSite.Models.Bookings;
using TravelSite.Models.Notification;

namespace TravelSite.Services
{
	public interface INotificationService
	{
		public Task RemoveNotificationAsync(Guid id);
		public Task<NotificationViewModel> GetNotificationAsync(Guid id);
		public Task<List<NotificationViewModel>> GetAllNotificationAsync();
		public Task<List<NotificationViewModel>> GetAllNotificationsByUserAsync(string id);
		public Task MarkNotificationAsDeliveredAsync(Guid id);
		public Task RemoveAllNotificationByUserAsync(string userId);
		public Task CreateBookingNotification(Guid bookId, string userId, string bookNum,string linkedUrl);
		public Task ConfirmBookingNotification(Guid id, string senderId, string bookNum, string linkedUrl);
		public Task CancelBookingNotification(Guid id, string senderId, string bookNum, string linkedUrl);
	}
}
