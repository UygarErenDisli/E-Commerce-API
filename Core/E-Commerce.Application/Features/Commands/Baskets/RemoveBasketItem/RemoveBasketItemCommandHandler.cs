using E_Commerce.Application.Abstractions.Basket;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Baskets.RemoveBasketItem
{
	public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommandRequest, RemoveBasketItemCommandResponse>
	{
		private readonly IBasketService _basketService;

		public RemoveBasketItemCommandHandler(IBasketService basketService)
		{
			_basketService = basketService;
		}

		public async Task<RemoveBasketItemCommandResponse> Handle(RemoveBasketItemCommandRequest request, CancellationToken cancellationToken)
		{
			await _basketService.RemoveItemAsync(request.BasketItemId);
			return new();
		}
	}
}
