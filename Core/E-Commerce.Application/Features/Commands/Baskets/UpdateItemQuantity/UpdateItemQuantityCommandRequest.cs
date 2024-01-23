using MediatR;

namespace E_Commerce.Application.Features.Commands.Baskets.UpdateItemQuantity
{
	public class UpdateItemQuantityCommandRequest : IRequest<UpdateItemQuantityCommandResponse>
	{
		public string BasketItemId { get; set; }
		public int Quantity { get; set; }
	}
}