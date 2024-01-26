using E_Commerce.Application.Abstractions.Hubs;
using E_Commerce.Application.Abstractions.Services.Baskets;
using E_Commerce.Application.Abstractions.Services.Order;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Orders.CreateOrder
{
	public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
	{
		private readonly IOrderService _orderService;
		private readonly IBasketService _basketService;
		private readonly IOrderHubService _orderHubService;

		public CreateOrderCommandHandler(IOrderService orderService, IBasketService basketService, IOrderHubService orderHubService)
		{
			_orderService = orderService;
			_basketService = basketService;
			_orderHubService = orderHubService;
		}

		public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
		{
			await _orderService.CreateOrderAsync(new()
			{
				BasketId = (await _basketService.GetActiveBasketAsync()).Id,
				City = request.City,
				Country = request.Country
					,
				State = request.State,
				Street = request.Street,
				Description = request.Description,
				ZipCode = request.ZipCode
			});
			await _orderHubService.OrderCreatedMessageAsync("A NEW ORDER RECEIVED!");
			return new();
		}
	}
}
