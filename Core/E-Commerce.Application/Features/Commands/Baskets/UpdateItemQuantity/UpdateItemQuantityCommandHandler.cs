using E_Commerce.Application.Abstractions.Services.Baskets;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Baskets.UpdateItemQuantity
{
    public class UpdateItemQuantityCommandHandler : IRequestHandler<UpdateItemQuantityCommandRequest, UpdateItemQuantityCommandResponse>
	{
		private readonly IBasketService _basketService;

		public UpdateItemQuantityCommandHandler(IBasketService basketService)
		{
			_basketService = basketService;
		}

		public async Task<UpdateItemQuantityCommandResponse> Handle(UpdateItemQuantityCommandRequest request, CancellationToken cancellationToken)
		{
			await _basketService.UpdateQuantityAsync(request.BasketItemId, request.Quantity);
			return new();
		}
	}
}
