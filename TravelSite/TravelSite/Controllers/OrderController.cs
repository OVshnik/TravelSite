using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelSite.Models.Orders;
using TravelSite.Services;

namespace TravelSite.Controllers
{
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;
		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}
		[HttpGet]
		[Route("PrepareAddOrder")]
		public async Task<IActionResult> PrepareAddOrder(Guid id)
		{
			var model = await _orderService.AddOrderAsync(id);
			return View("AddOrder", model);
		}
		[HttpPost]
		[Route("AddOrder")]
		public async Task<IActionResult> AddOrder(CreateOrderViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _orderService.AddOrderAsync(model);
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("PrepareAddOrder");
		}
		[Authorize("Admin")]
		[HttpGet]
		[Route("GetAllOrders")]
		public async Task<IActionResult> GetAllOrders()
		{
			var model = await _orderService.GetAllOrdersAsync();
			return View("OrderList",model);
		}
		[Authorize("Admin")]
		[HttpGet]
		[Route("GetOrder")]
		public async Task<IActionResult> GetOrder(Guid id)
		{
			var model = await _orderService.GetOrderByIdAsync(id);
			return View("OrderPage",model);
		}
		[Authorize("Admin")]
		[HttpGet]
		[Route("EditOrder")]
		public async Task<IActionResult> EditOrder(Guid id)
		{
			var model = await _orderService.EditOrderAsync(id);
			return View("EditOrder",model);
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("UpdateOrder")]
		public async Task<IActionResult> UpdateOrder(EditOrderViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _orderService.UpdateOrderAsync(model);
				return RedirectToAction("GetAllOrders");
			}
			return RedirectToAction("EditOrder");
		}
		[Authorize("Admin")]
		[HttpPost]
		[Route("DeleteOrder")]
		public async Task<IActionResult> DeleteOrder(Guid id)
		{
			await _orderService.RemoveOrderAsync(id);
			return RedirectToAction("GetAllOrders");
		}
	}
}
