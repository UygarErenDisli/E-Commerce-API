using MediatR;

namespace E_Commerce.Application.Features.Commands.Baskets.AddItemToBasket
{
	public class AddItemToBasketCommandRequest : IRequest<AddItemToBasketCommandResponse>
	{
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}