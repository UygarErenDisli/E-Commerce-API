using E_Commerce.Application.Abstractions.Services.Order;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.Orders.GetByIdOrder
{
	public class GetByIdOrderQueryHandler : IRequestHandler<GetByIdOrderQueryRequest, GetByIdOrderQueryResponse>
	{
		private readonly IOrderService _orderService;

		public GetByIdOrderQueryHandler(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public async Task<GetByIdOrderQueryResponse> Handle(GetByIdOrderQueryRequest request, CancellationToken cancellationToken)
		{
			var orderDto = await _orderService.GetOrderByIdAsync(request.Id);

			return new()
			{
				Id = orderDto.Id,
				OrderCode = orderDto.OrderCode,
				CreatedDate = orderDto.CreatedDate,
				TotalPrice = orderDto.TotalPrice,
				Description = orderDto.Description,
				UserEmail = orderDto.UserEmail,
				UserName = orderDto.UserName,
				Address = orderDto.Address,
				BasketItems = orderDto.BasketItems
			};
		}
	}
}
