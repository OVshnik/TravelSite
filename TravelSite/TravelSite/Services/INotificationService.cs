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
	}
}
