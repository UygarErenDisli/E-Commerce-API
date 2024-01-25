using E_Commerce.Application.Abstractions.Services.Baskets;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.Baskets.GetAllBasketItems
{
    public class GetAllBasketItemsQueryHandler : IRequestHandler<GetAllBasketItemsQueryRequest, List<GetAllBasketItemsQueryResponse>>
	{
		private readonly IBasketService _basketService;

		public GetAllBasketItemsQueryHandler(IBasketService basketService)
		{
			_basketService = basketService;
		}

		public async Task<List<GetAllBasketItemsQueryResponse>> Handle(GetAllBasketItemsQueryRequest request, CancellationToken cancellationToken)
		{
			var response = await _basketService.GetBasketItemsAsync();
			return response.Select(item => new GetAllBasketItemsQueryResponse()
			{
				BasketItemId = item.Id.ToString(),
				Name = item.Product.Name,
				Price = item.Product.Price,
				Quantity = item.Quantity
			}).ToList();
		}
	}
}
