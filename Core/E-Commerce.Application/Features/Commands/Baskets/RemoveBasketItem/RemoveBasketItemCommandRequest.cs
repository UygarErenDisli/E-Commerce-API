using MediatR;

namespace E_Commerce.Application.Features.Commands.Baskets.RemoveBasketItem
{
	public class RemoveBasketItemCommandRequest : IRequest<RemoveBasketItemCommandResponse>
	{
		public string BasketItemId { get; set; }
	}
}