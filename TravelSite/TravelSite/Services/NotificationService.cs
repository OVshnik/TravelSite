using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Notification;

namespace TravelSite.Services
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository<BookingNotification> _notificationRepository;
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;
		public NotificationService(INotificationRepository<BookingNotification> notificationRepository,
			IMapper mapper, 
			UserManager<User> userManager)
		{
			_notificationRepository = notificationRepository;
			_mapper = mapper;
			_userManager = userManager;
		}
		public async Task<List<NotificationViewModel>> GetAllNotificationAsync()
		{
			var nots=await _notificationRepository.GetAllNotificationsAsync();
			var notList=new List<NotificationViewModel>();
			if (nots!=null)
			{
				foreach(var notification in nots)
				{
					notList.Add(_mapper.Map<NotificationViewModel>(notification));
				}
			}
			return notList;
		}
		public async Task<List<NotificationViewModel>> GetAllNotificationsByUserAsync(string id)
		{
			var user= await _userManager.FindByIdAsync(id);
			var nots = await _notificationRepository.GetAllNotificationsAsync();
			var notList = new List<NotificationViewModel>();
			if (nots != null&&user!=null)
			{
				nots = nots.Where(x => x.RecipientId == id).ToList();
				foreach (var notification in nots)
				{
					notList.Add(_mapper.Map<NotificationViewModel>(notification));
				}
			}
			return notList;
		}
		public async Task<NotificationViewModel> GetNotificationAsync(Guid id)
		{
			var notification=await _notificationRepository.GetNotificationByIdAsync(id);
			if (notification!=null)
			{
				var model=_mapper.Map<NotificationViewModel>(notification);
				return model;
			}
			throw new NotImplementedException();
		}

		public async Task RemoveNotificationAsync(Guid id)
		{
			await _notificationRepository.DeleteNotificationAsync(id);
		}

		public async Task RemoveAllNotificationByUserAsync(string userId)
		{
			var nots=await _notificationRepository.GetAllNotificationsAsync();
			if(nots!=null)
			{
				var removeNots = nots.Where(x => x.RecipientId == userId);
				foreach (var notification in removeNots)
				{
					await _notificationRepository.DeleteNotificationAsync(notification.Id);
				}
			}
		}
		public async Task MarkNotificationAsDeliveredAsync(Guid id)
		{
			var not=await _notificationRepository.GetNotificationByIdAsync(id);
			if (not!=null)
			{
				not.Delivered = true;
				await _notificationRepository.UpdateNotificationAsync(not);
			}
		}
	}
}
