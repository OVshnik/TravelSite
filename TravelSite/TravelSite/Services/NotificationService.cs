using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Notification;

namespace TravelSite.Services
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository<BookingNotification> _notificationRepository;
		private readonly IBookingRepository _bookingRepository;
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;
		private readonly IEmailService _emailService;
		public NotificationService(INotificationRepository<BookingNotification> notificationRepository,
			IMapper mapper,
			UserManager<User> userManager,
			IBookingRepository bookingRepository,
			IEmailService emailService)
		{
			_notificationRepository = notificationRepository;
			_mapper = mapper;
			_userManager = userManager;
			_bookingRepository = bookingRepository;
			_emailService = emailService;
		}
		public async Task CreateBookingNotification(Guid bookId, string userId, string bookNum, string linkedUrl)
		{
			var admins = await _userManager.GetUsersInRoleAsync("Admin");

			var admin = admins.FirstOrDefault();

			var message = $"Бронирование №{bookNum} создано";

			var booking = await _bookingRepository.GetBookingByIdAsync(bookId);
			if (booking != null&&admin!=null)
			{
				var notification = new BookingNotification
				{
					Content = message,
					RecipientId = admin.Id,
					SenderId = userId,
					BookingId = booking.Id
				};
				await _notificationRepository.CreateNotificationAsync(notification);
				message = message + $"\n Перейти {linkedUrl}";
				var sender = await _userManager.FindByIdAsync(userId);
				await _emailService.SendEmailAsync(admin.Email, sender.Email, "nnnl vaei vqyv anmp", message, bookNum);
			}
		}
		public async Task ConfirmBookingNotification(Guid id, string senderId, string bookNum, string linkedUrl)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			if (booking != null)
			{
				booking.BookingStatus = "Confirmed";
				await _bookingRepository.UpdateBookingAsync(booking);
				var message = $"Бронирование №{bookNum} подтверждено";
				var notification = new BookingNotification
				{
					Content = message,
					RecipientId = booking.UserId,
					SenderId = senderId,
					BookingId = booking.Id
				};
				await _notificationRepository.CreateNotificationAsync(notification);
				message = message+$"\n Перейти {linkedUrl}";
				var recipient = await _userManager.FindByIdAsync(booking.UserId);
				await _emailService.SendEmailAsync(recipient.Email, recipient.Email, "nnnl vaei vqyv anmp", message, bookNum);
			}
			else
				throw new Exception($"Бронирование с id'{id}'не найдено");
		}
		public async Task CancelBookingNotification(Guid id, string senderId, string bookNum, string linkedUrl)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);
			if (booking != null)
			{
				booking.BookingStatus = "Canceled";
				await _bookingRepository.UpdateBookingAsync(booking);
				var message = $"Бронирование №{bookNum} отменено";
				var notification = new BookingNotification
				{
					Content = message,
					RecipientId = booking.UserId,
					SenderId = senderId,
					BookingId = booking.Id
				};
				await _notificationRepository.CreateNotificationAsync(notification);
				message = message + $"\n Перейти {linkedUrl}";
				var recipient = await _userManager.FindByIdAsync(booking.UserId);
				var sender = await _userManager.FindByIdAsync(senderId);
				await _emailService.SendEmailAsync(recipient.Email, sender.Email, "nnnl vaei vqyv anmp", message, bookNum);
			}
			else
				throw new Exception($"Бронирование с id'{id}'не найдено");
		}
		public async Task<List<NotificationViewModel>> GetAllNotificationAsync()
		{
			var nots = await _notificationRepository.GetAllNotificationsAsync();
			var notList = new List<NotificationViewModel>();
			if (nots != null)
			{
				foreach (var notification in nots)
				{
					notList.Add(_mapper.Map<NotificationViewModel>(notification));
				}
			}
			return notList;
		}
		public async Task<List<NotificationViewModel>> GetAllNotificationsByUserAsync(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var nots = await _notificationRepository.GetAllNotificationsAsync();
			var notList = new List<NotificationViewModel>();
			if (nots != null && user != null)
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
			var notification = await _notificationRepository.GetNotificationByIdAsync(id);
			if (notification != null)
			{
				var model = _mapper.Map<NotificationViewModel>(notification);
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
			var nots = await _notificationRepository.GetAllNotificationsAsync();
			if (nots != null)
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
			var not = await _notificationRepository.GetNotificationByIdAsync(id);
			if (not != null)
			{
				not.Delivered = true;
				await _notificationRepository.UpdateNotificationAsync(not);
			}
		}
	}
}
