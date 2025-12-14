using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public interface IOrderRepository
	{
		Task CreateOrderAsync(Order order);
		Task<List<Order>> GetAllOrdersAsync();
		Task <Order?> GetOrderByIdAsync(Guid id);
		Task UpdateOrderAsync(Order order);
		Task DeleteOrderAsync(Guid id);	
	}
}
