using E_Commerce.Application.Abstractions.Basket;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Baskets.AddItemToBasket
{
	public class AddItemToBasketCommandHandler : IRequestHandler<AddItemToBasketCommandRequest, AddItemToBasketCommandResponse>
	{
		private readonly IBasketService _basketService;

		public AddItemToBasketCommandHandler(IBasketService basketService)
		{
			_basketService = basketService;
		}

		public async Task<AddItemToBasketCommandResponse> Handle(AddItemToBasketCommandRequest request, CancellationToken cancellationToken)
		{
			await _basketService.AddItemToBasketAsync(request.ProductId, request.Quantity);

			return new();
		}
	}
}
