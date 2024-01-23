using MediatR;

namespace E_Commerce.Application.Features.Quaries.Baskets.GetAllBasketItems
{
	public class GetAllBasketItemsQueryRequest : IRequest<List<GetAllBasketItemsQueryResponse>>
	{
	}
}