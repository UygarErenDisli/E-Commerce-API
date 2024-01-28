using E_Commerce.Application.Abstractions.Services.Order;
using E_Commerce.Application.DTOs.BasketItem;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.Exceptions;
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

		public async Task<SingleDetailedOrderDTO> GetOrderByIdAsync(string id)
		{
			var query = _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.Include(o => o.Basket)
				.ThenInclude(b => b.User);

			var data = await query.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));
			if (data == null)
			{
				throw new OrderNotFoundException();

			}
			return new()
			{
				Id = data.Id.ToString(),
				OrderCode = data.OrderCode,
				CreatedDate = data.CreatedDate,
				Description = data.Description,
				UserEmail = data.Basket.User.Email!,
				UserName = data.Basket.User.UserName!,
				TotalPrice = data.Basket.BasketItems.Sum(i => i.Product.Price * i.Quantity),
				Address = new()
				{
					City = data.Address.City,
					Country = data.Address.Country,
					State = data.Address.State,
					Street = data.Address.Street,
					ZipCode = data.Address.ZipCode,
				},
				BasketItems = data.Basket.BasketItems.Select(bi => new BasketItemDTO()
				{
					Price = bi.Product.Price,
					ProductId = bi.Product.Id.ToString(),
					ProductName = bi.Product.Name,
					Quantity = bi.Quantity
				}).ToList(),
			};
		}
	}
}
