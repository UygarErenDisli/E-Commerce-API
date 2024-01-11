using MediatR;

namespace E_Commerce.Application.Features.Commands.Products.DeleteProduct
{
	public class DeleteProductCommandRequest : IRequest<DeleteProductCommandResponse>
	{
		public string Id { get; set; }
	}
}
