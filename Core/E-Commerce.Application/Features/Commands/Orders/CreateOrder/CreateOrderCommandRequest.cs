using MediatR;

namespace E_Commerce.Application.Features.Commands.Orders.CreateOrder
{
	public class CreateOrderCommandRequest : IRequest<CreateOrderCommandResponse>
	{
		public string Description { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
	}
}
