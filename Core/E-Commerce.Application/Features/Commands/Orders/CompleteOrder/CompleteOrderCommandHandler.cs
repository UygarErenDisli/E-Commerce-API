using E_Commerce.Application.Abstractions.Services.Notification;
using E_Commerce.Application.Abstractions.Services.Order;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Orders.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
	{
		private readonly IOrderService _orderService;
		private readonly INotificationService _notificationService;

		public CompleteOrderCommandHandler(IOrderService orderService, INotificationService notificationService)
		{
			_orderService = orderService;
			_notificationService = notificationService;
		}

		public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
		{
			var completedOrderDto = await _orderService.CompleteOrderAsync(request.Id);

			await _notificationService.CreateOrderCompletedNotificationAsync(new()
			{
				OrderCode = completedOrderDto.OrderCode,
				UserName = completedOrderDto.UserName,
				OrderDate = completedOrderDto.OrderDate,
				UserId = completedOrderDto.UserId
			});

			return new();
		}
	}
}
