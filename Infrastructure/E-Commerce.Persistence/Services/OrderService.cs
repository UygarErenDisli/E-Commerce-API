using E_Commerce.Application.Abstractions.Services.Order;
using E_Commerce.Application.DTOs.BasketItem;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.Exceptions;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace E_Commerce.Persistence.Services
{
	public class OrderService : IOrderService
	{
		private const int _orderCodeLength = 12;

		private readonly IOrderWriteRepository _orderWriteRepository;
		private readonly IOrderReadRepository _orderReadRepository;
		private readonly ICompletedOrderReadRepository _completedOrderReadRepository;
		private readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
		private readonly ICanceledOrderReadRepository _canceledOrderReadRepository;
		private readonly ICanceledOrderWriteRepository _canceledOrderWriteRepository;

		public OrderService(IOrderWriteRepository orderWriteRepository,
					  IOrderReadRepository orderReadRepository,
					  ICompletedOrderReadRepository completedOrderReadRepository,
					  ICompletedOrderWriteRepository completedOrderWriteRepository,
					  ICanceledOrderReadRepository canceledOrderReadRepository,
					  ICanceledOrderWriteRepository canceledOrderWriteRepository)
		{
			_orderWriteRepository = orderWriteRepository;
			_orderReadRepository = orderReadRepository;
			_completedOrderReadRepository = completedOrderReadRepository;
			_completedOrderWriteRepository = completedOrderWriteRepository;
			_canceledOrderReadRepository = canceledOrderReadRepository;
			_canceledOrderWriteRepository = canceledOrderWriteRepository;
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
			var query = GetIncludedQuery();

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
					IsCompleted = order.IsCompleted,
					IsCanceled = order.IsCanceled,
				})
				.ToListAsync();

			return data;
		}
		public async Task<SingleDetailedOrderDTO> GetOrderByIdAsync(string id)
		{
			var query = GetIncludedQuery();

			var data = await query.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id)) ?? throw new OrderNotFoundException();
			var output = new SingleDetailedOrderDTO()
			{
				Id = data.Id.ToString(),
				OrderCode = data.OrderCode,
				CreatedDate = data.CreatedDate,
				Description = data.Description,
				UserEmail = data.Basket.User.Email!,
				UserName = data.Basket.User.UserName!,
				IsCompleted = data.IsCompleted,
				IsCanceled = data.IsCanceled,
				ReasonforCancellation = !string.IsNullOrEmpty(data.ReasonforCancellation) ? data.ReasonforCancellation : "No Reason Given For Cancellation",
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

			return output;
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
		public async Task<OrderCanceledDTO> CancelOrderAsync(string orderId, string reason)
		{
			var query = _orderWriteRepository.Table.Include(o => o.Basket).ThenInclude(b => b.User);

			var order = await query.FirstOrDefaultAsync(o => o.Id == Guid.Parse(orderId));

			if (order != null)
			{
				await _canceledOrderWriteRepository.AddAsync(new() { OrderId = order.Id, Reason = reason });
				var result = await _canceledOrderWriteRepository.SaveAsync();
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
		private IQueryable<OrderQuery> GetIncludedQuery()
		{
			var includedOrderQuery = _orderReadRepository.Table
				.Include(o => o.Basket)
				.ThenInclude(b => b.BasketItems)
				.ThenInclude(bi => bi.Product)
				.Include(o => o.Basket)
				.ThenInclude(b => b.User)
				.Include(o => o.Address);


			var completedOrderQuery = from order in includedOrderQuery
									  join completedOrder in _completedOrderReadRepository.Table
									  on order.Id equals completedOrder.OrderId into completedOrderJoinedTable
									  from includedCompleteOrder in completedOrderJoinedTable.DefaultIfEmpty()
									  select new
									  {
										  order.Id,
										  order.Basket,
										  order.CreatedDate,
										  order.OrderCode,
										  order.Description,
										  order.Address.State,
										  order.Address.City,
										  order.Address.Country,
										  order.Address.ZipCode,
										  order.Address.Street,
										  IsCompleted = includedCompleteOrder != null,
									  };


			var canceledOrderQuery = from order in includedOrderQuery
									 join canceledOrder in _canceledOrderReadRepository.Table
									 on order.Id equals canceledOrder.OrderId into canceledOrderJoinedTable
									 from includedCanceledOrder in canceledOrderJoinedTable.DefaultIfEmpty()
									 select new
									 {
										 order.Id,
										 order.Basket,
										 order.CreatedDate,
										 order.OrderCode,
										 IsCanceled = includedCanceledOrder != null,
										 Reason = includedCanceledOrder != null ? includedCanceledOrder.Reason : ""
									 };

			var output = from co in completedOrderQuery
						 join ca in canceledOrderQuery
						 on co.Id equals ca.Id into caJoinedTable
						 from order in caJoinedTable.DefaultIfEmpty()
						 orderby order.CreatedDate
						 select new OrderQuery()
						 {
							 Id = order.Id,
							 Basket = order.Basket,
							 CreatedDate = order.CreatedDate,
							 OrderCode = order.OrderCode,
							 ZipCode = co.ZipCode,
							 Street = co.Street,
							 City = co.City,
							 Country = co.Country,
							 State = co.State,
							 Description = co.Description,
							 IsCompleted = co.IsCompleted,
							 IsCanceled = order.IsCanceled,
							 ReasonforCancellation = order.Reason,
						 };

			return output;
		}
		private class OrderQuery
		{
			public Guid Id { get; set; }
			public Basket Basket { get; set; }
			public DateTime CreatedDate { get; set; }
			public string OrderCode { get; set; }
			public string Description { get; set; }
			public bool IsCompleted { get; set; }
			public bool IsCanceled { get; set; }
			public string ReasonforCancellation { get; set; }
			public string Street { get; set; }
			public string City { get; set; }
			public string State { get; set; }
			public string Country { get; set; }
			public string ZipCode { get; set; }
		}
		private string CreateOrderCode()
		{
			byte[] number = new byte[_orderCodeLength];
			using var numberGenerator = RandomNumberGenerator.Create();
			numberGenerator.GetBytes(number);
			return Convert.ToBase64String(number);

		}

	}
}
