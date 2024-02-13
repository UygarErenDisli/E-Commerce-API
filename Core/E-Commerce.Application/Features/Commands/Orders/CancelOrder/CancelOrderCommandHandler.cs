using E_Commerce.Application.Abstractions.Services.Notification;
using E_Commerce.Application.Abstractions.Services.Order;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Orders.CancelOrder
{
	public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommandRequest, CancelOrderCommandResponse>
	{
		private readonly IOrderService _orderService;
		private readonly INotificationService _notificationService;

		public CancelOrderCommandHandler(IOrderService orderService, INotificationService notificationService)
		{
			_orderService = orderService;
			_notificationService = notificationService;
		}

		public async Task<CancelOrderCommandResponse> Handle(CancelOrderCommandRequest request, CancellationToken cancellationToken)
		{
			var orderCanceledDto = await _orderService.CancelOrderAsync(request.Id, request.Reason);
			await _notificationService.CreateOrderCanceledNotificationAsync(new()
			{
				OrderCode = orderCanceledDto.OrderCode,
				OrderDate = orderCanceledDto.OrderDate,
				ReasonforCancellation = request.Reason,
				UserName = orderCanceledDto.UserName,
				UserId = orderCanceledDto.UserId
			});
			return new();
		}
	}
}
