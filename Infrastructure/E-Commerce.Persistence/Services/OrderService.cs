using E_Commerce.Application.Abstractions.Services.Order;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.Repositories;

namespace E_Commerce.Persistence.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderWriteRepository _orderWriteRepository;

		public OrderService(IOrderWriteRepository orderWriteRepository)
		{
			_orderWriteRepository = orderWriteRepository;
		}

		public async Task CreateOrderAsync(CreateOrderDTO createOrder)
		{
			var orderCode = (new Random().NextDouble() * 10000).ToString();
			orderCode = orderCode.Substring(orderCode.IndexOf('.') + 1, orderCode.Length - orderCode.IndexOf('.') - 1);

			await _orderWriteRepository.AddAsync(new()
			{
				Id = createOrder.BasketId,
				Address = new()
				{
					City = createOrder.City,
					Country = createOrder.Country,
					ZipCode = createOrder.ZipCode,
					State = createOrder.State,
					Street = createOrder.Street
				},
				Description = createOrder.Description,
				OrderCode = orderCode
			});
			await _orderWriteRepository.SaveAsync();
		}
	}
}
