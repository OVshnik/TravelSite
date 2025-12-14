using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public interface INotificationRepository<T> where T: Notification
	{
		Task CreateNotificationAsync(T notification);
		Task<List<T>> GetAllNotificationsAsync();
		Task<T?> GetNotificationByIdAsync(Guid id);
		Task UpdateNotificationAsync(T notification);
		Task DeleteNotificationAsync(Guid id);
	}
}
