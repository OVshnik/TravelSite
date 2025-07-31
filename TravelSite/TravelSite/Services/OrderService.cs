using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Bookings;
using TravelSite.Models.Orders;
using TravelSite.Models.Travels;

namespace TravelSite.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IBookingRepository _bookingRepository;
		private readonly ITravelRepository _travelRepository;
		private readonly ITravelDatesRepository _travelDatesRepository;
		private readonly IMapper _mapper;
		public OrderService(IOrderRepository orderRepository,
			IMapper mapper,
			IBookingRepository bookingRepository,
			ITravelRepository travelRepository,
			ITravelDatesRepository travelDatesRepository)
		{
			_orderRepository = orderRepository;
			_mapper = mapper;
			_bookingRepository = bookingRepository;
			_travelRepository = travelRepository;
			_travelDatesRepository = travelDatesRepository;
		}

		public async Task<CreateOrderViewModel> AddOrderAsync(Guid id)
		{
			var booking = await _bookingRepository.GetBookingByIdAsync(id);

			if (booking != null)
			{
				var travel = await _travelRepository.GetTravelByIdAsync(booking.TravelId);
				var trDates = await _travelDatesRepository.GetTravelDatesByIdAsync(booking.TravelDatesId);

				if (trDates != null&&travel!=null)
				{
					var model = new CreateOrderViewModel()
					{
						OrderNumber = await GenerateOrderNumber(travel.Category,booking.UserId),
						BookingId = booking.Id,
						TotalPrice = trDates.Price,
						Travel = _mapper.Map<TravelViewModel>(travel),
					};
					return model;
				}
			}
			throw new Exception($"Бронирования с таким id {id} не найдено в БД");
		}

		public async Task AddOrderAsync(CreateOrderViewModel model)
		{
			var order = _mapper.Map<Order>(model);

			var booking = await _bookingRepository.GetBookingByIdAsync(model.BookingId);
			if (booking != null)
			{
				order.Booking = booking;
				
				await _orderRepository.CreateOrderAsync(order);
			}	
		}

		public async Task<EditOrderViewModel> EditOrderAsync(Guid id)
		{
			var order = await _orderRepository.GetOrderByIdAsync(id);
			if (order != null)
			{
				var model = _mapper.Map<EditOrderViewModel>(order);
				return model;
			}
			throw new Exception($"Заказ с таким id {id} не найден в БД");
		}
		public async Task UpdateOrderAsync(EditOrderViewModel model)
		{
			var order = await _orderRepository.GetOrderByIdAsync(model.Id);
			if (order != null)
			{
				order.CreateDate = model.CreateDate;
				if (!string.IsNullOrEmpty(model.Description))
					order.Description = model.Description;
				if (!string.IsNullOrEmpty(model.Status))
					order.Status = model.Status;

				await _orderRepository.UpdateOrderAsync(order);
			}
		}
		public async Task<List<OrderViewModel>> GetAllOrdersAsync()
		{
			var listOrder = new List<OrderViewModel>();
			var orders = await _orderRepository.GetAllOrdersAsync();

			foreach (var order in orders)
			{
				listOrder.Add(_mapper.Map<OrderViewModel>(order));
			}
			return listOrder;
		}

		public async Task<OrderViewModel> GetOrderByIdAsync(Guid id)
		{
			var order = await _orderRepository.GetOrderByIdAsync(id);
			if (order != null)
				return _mapper.Map<OrderViewModel>(order);
			throw new Exception($"Заказ с таким id {id} не найден в БД");
		}

		public async Task RemoveOrderAsync(Guid id)
		{
			var order = await _orderRepository.GetOrderByIdAsync(id);
			if (order != null)
				await _orderRepository.DeleteOrderAsync(id);
		}
		public async Task<string> GenerateOrderNumber(string trName, string userId)
		{
			if(!string.IsNullOrEmpty(userId)&&!string.IsNullOrEmpty(trName))
			{

				var idFragment = userId.Substring(userId.Length - 5);

				var rndNum = new Random().Next(0, 99999).ToString();

				var orderNum = "C" + trName[0].ToString().ToUpper() + "-" + rndNum + "-" + idFragment;

				var orders = await _orderRepository.GetAllOrdersAsync();

				var check=orders.Where(x => x.OrderNumber == orderNum).FirstOrDefault();

				if(check==null)
				{
					return orderNum;
				}
				return await GenerateOrderNumber(trName, userId);
			}
			throw new NullReferenceException();
		}
	}
}
