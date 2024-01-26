using E_Commerce.Application.Abstractions.Services.Order;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.Orders.GetAllOrders
{
	public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
	{
		private readonly IOrderService _orderService;

		public GetAllOrdersQueryHandler(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
		{
			var totalCount = _orderService.GetTotalOrdersCount();
			var ordersDTO = await _orderService.GetAllOrdersAsync(request.PageIndex, request.PageSize);

			return new()
			{
				Orders = ordersDTO,
				TotalCount = totalCount
			};
		}
	}
}
