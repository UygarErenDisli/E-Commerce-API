using MediatR;

namespace E_Commerce.Application.Features.Commands.Products.CreateProduct
{
	public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
	{
		public string Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
	}
}
