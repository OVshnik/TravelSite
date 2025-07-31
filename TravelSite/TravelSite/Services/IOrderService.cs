using System.ComponentModel.DataAnnotations;
using TravelSite.Models.Orders;

namespace TravelSite.Services
{
	public interface IOrderService
	{
		public Task<CreateOrderViewModel> AddOrderAsync(Guid id);
		public Task AddOrderAsync(CreateOrderViewModel model);
		public Task<OrderViewModel> GetOrderByIdAsync(Guid id);
		public Task<List<OrderViewModel>> GetAllOrdersAsync();
		public Task<EditOrderViewModel> EditOrderAsync(Guid id);
		public Task UpdateOrderAsync(EditOrderViewModel model);
		public Task RemoveOrderAsync(Guid id);	
	}
}
