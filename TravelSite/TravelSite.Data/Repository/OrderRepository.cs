using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public class OrderRepository : IOrderRepository
	{
		private ApplicationDbContext _context;
		public OrderRepository(ApplicationDbContext db)
		{
		    _context = db;
		}
		public async Task CreateOrderAsync(Order order)
		{
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
		}
		public async Task DeleteOrderAsync(Guid id)
		{
			var order = await GetOrderByIdAsync(id);
			if (order != null)
			{
				_context.Orders.Remove(order);
			}
			await _context.SaveChangesAsync();
		}
		public async Task<List<Order>> GetAllOrdersAsync()
		{
			var orders=await _context.Orders.ToListAsync();
			return orders;
		}
		public async Task<Order?> GetOrderByIdAsync(Guid id)
		{
			var order = await _context.Orders.Where(x=>x.Id==id).FirstOrDefaultAsync();
			return order;
		}
		public async Task UpdateOrderAsync(Order order)
		{
			_context.Update(order);
			await _context.SaveChangesAsync();	
		}
	}
}
