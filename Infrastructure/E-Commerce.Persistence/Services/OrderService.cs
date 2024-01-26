using E_Commerce.Application.Abstractions.Services.Order;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace E_Commerce.Persistence.Services
{
	public class OrderService : IOrderService
	{
		private const int OrderCodeLength = 12;
		private readonly IOrderWriteRepository _orderWriteRepository;
		private readonly IOrderReadRepository _orderReadRepository;

		public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
		{
			_orderWriteRepository = orderWriteRepository;
			_orderReadRepository = orderReadRepository;
		}

		public int GetTotalOrdersCount() => _orderReadRepository.GetAll().Count();

		public async Task CreateOrderAsync(CreateOrderDTO createOrder)
		{
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
				OrderCode = CreateOrderCode()
			});
			await _orderWriteRepository.SaveAsync();
		}

		public async Task<List<ListOrderDTO>> GetAllOrdersAsync(int pageIndex, int pageSize)
		{
			var query = _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.Include(o => o.Basket)
				.ThenInclude(b => b.User);

			var data = await query
				.Skip(pageIndex * pageSize).Take(pageSize)
				.Select(order => new ListOrderDTO()
				{
					Id = order.Id.ToString(),
					CreatedDate = order.CreatedDate,
					OrderCode = order.OrderCode,
					TotalPrice = order.Basket.BasketItems.Sum(b => b.Product.Price * b.Quantity),
					UserEmail = order.Basket.User.Email!,
					Username = order.Basket.User.UserName!,
				})
				.ToListAsync();

			return data;
		}

		private string CreateOrderCode()
		{
			byte[] number = new byte[OrderCodeLength];
			using var numberGenerator = RandomNumberGenerator.Create();
			numberGenerator.GetBytes(number);
			return Convert.ToBase64String(number);

		}
	}
}
