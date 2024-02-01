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
		private readonly ICompletedOrderReadRepository _completedOrderReadRepository;
		private readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;

		public OrderService(
			IOrderWriteRepository orderWriteRepository,
			IOrderReadRepository orderReadRepository,
			ICompletedOrderReadRepository completedOrderReadRepository,
			ICompletedOrderWriteRepository completedOrderWriteRepository)
		{
			_orderWriteRepository = orderWriteRepository;
			_orderReadRepository = orderReadRepository;
			_completedOrderReadRepository = completedOrderReadRepository;
			_completedOrderWriteRepository = completedOrderWriteRepository;
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
			var includedOrderQuery = _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.Include(o => o.Basket)
				.ThenInclude(b => b.User);


			var joinedQuery = from order in includedOrderQuery
							  join completedOrder in _completedOrderReadRepository.Table
							  on order.Id equals completedOrder.OrderId into completedOrderJoinedTable
							  from includedCompleteOrder in completedOrderJoinedTable.DefaultIfEmpty()
							  select new
							  {
								  order.Id,
								  order.Basket,
								  order.CreatedDate,
								  order.OrderCode,
								  IsCompleted = includedCompleteOrder != null
							  };


			var data = await joinedQuery
				.Skip(pageIndex * pageSize).Take(pageSize)
				.Select(order => new ListOrderDTO()
				{
					Id = order.Id.ToString(),
					CreatedDate = order.CreatedDate,
					OrderCode = order.OrderCode,
					TotalPrice = order.Basket.BasketItems.Sum(b => b.Product.Price * b.Quantity),
					UserEmail = order.Basket.User.Email!,
					Username = order.Basket.User.UserName!,
					IsCompleted = order.IsCompleted

				})
				.ToListAsync();

			return data;
		}



		public async Task<SingleDetailedOrderDTO> GetOrderByIdAsync(string id)
		{
			var includedOrderQuery = _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.Include(o => o.Basket)
				.ThenInclude(b => b.User);

			var joinedOrderQuery = from order in includedOrderQuery
								   join completedOrder in _completedOrderReadRepository.Table
								   on order.Id equals completedOrder.OrderId into completedOrderJoinedTable
								   from includedCompleteOrder in completedOrderJoinedTable.DefaultIfEmpty()
								   select new
								   {
									   order.Id,
									   order.OrderCode,
									   order.CreatedDate,
									   order.Description,
									   order.Basket,
									   order.Address.State,
									   order.Address.City,
									   order.Address.Country,
									   order.Address.ZipCode,
									   order.Address.Street,
									   IsCompleted = includedCompleteOrder != null
								   };


			var data = await joinedOrderQuery.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));
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
				IsCompleted = data.IsCompleted,
				TotalPrice = data.Basket.BasketItems.Sum(i => i.Product.Price * i.Quantity),
				Address = new()
				{
					City = data.City,
					Country = data.Country,
					State = data.State,
					Street = data.Street,
					ZipCode = data.ZipCode,
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

		public async Task<CompletedOrderDTO> CompleteOrderAsync(string id)
		{
			var incluededOrders = _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.User);

			var order = await incluededOrders.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

			if (order != null)
			{
				await _completedOrderWriteRepository.AddAsync(new() { OrderId = order.Id });
				var result = await _completedOrderWriteRepository.SaveAsync();
				if (result > 0)
				{
					return new()
					{
						OrderCode = order.OrderCode,
						OrderDate = order.CreatedDate,
						UserEmail = order.Basket.User.Email!,
						UserId = order.Basket.UserId,
						UserName = order.Basket.User.UserName!
					};
				}
				else
				{
					throw new Exception("Unexpected error occurred");
				}
			}
			else
			{
				throw new OrderNotFoundException();
			}
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
