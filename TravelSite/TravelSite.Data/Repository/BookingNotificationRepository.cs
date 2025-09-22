using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public class BookingNotificationRepository:INotificationRepository<BookingNotification>
	{
		private ApplicationDbContext _context;
		public BookingNotificationRepository(ApplicationDbContext db)
		{
			_context = db;
		}
		public async Task CreateNotificationAsync(BookingNotification notification)
		{
			_context.Notifications.Add(notification);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteNotificationAsync(Guid id)
		{
			var notification = await GetNotificationByIdAsync(id);
			if (notification != null)
			{
				_context.Notifications.Remove(notification);
			}
			await _context.SaveChangesAsync();
		}

		public async Task<List<BookingNotification>> GetAllNotificationsAsync()
		{
			var notifications = await _context.Notifications.AsQueryable().
				Include(x => x.Booking).
				Include(y => y.Sender).
				Include(y => y.Recipient).ToListAsync();
			return notifications;
		}

		public async Task<BookingNotification?> GetNotificationByIdAsync(Guid id)
		{
			var notifications = await _context.Notifications.Where(x => x.Id == id).
				Include(x => x.Booking).
				Include(y => y.Sender).
				Include(y => y.Recipient).FirstOrDefaultAsync();
			return notifications;
		}

		public async Task UpdateNotificationAsync(BookingNotification notification)
		{
			_context.Update(notification);
			await _context.SaveChangesAsync();
		}
	}
}
