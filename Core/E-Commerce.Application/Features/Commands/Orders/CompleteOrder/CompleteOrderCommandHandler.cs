using E_Commerce.Application.Abstractions.Services.Order;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Orders.CompleteOrder
{
	public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
	{
		private IOrderService _orderService;

		public CompleteOrderCommandHandler(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
		{
			await _orderService.CompleteOrderAsync(request.Id);
			return new();
		}
	}
}
