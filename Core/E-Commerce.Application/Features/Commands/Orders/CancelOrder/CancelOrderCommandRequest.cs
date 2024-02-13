using MediatR;

namespace E_Commerce.Application.Features.Commands.Orders.CancelOrder
{
	public class CancelOrderCommandRequest : IRequest<CancelOrderCommandResponse>
	{
		public string Id { get; set; }
		public string Reason { get; set; }
	}
}