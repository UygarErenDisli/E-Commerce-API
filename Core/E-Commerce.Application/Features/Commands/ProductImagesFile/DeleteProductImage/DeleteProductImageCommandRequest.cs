using MediatR;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.DeleteProductImage
{
	public class DeleteProductImageCommandRequest : IRequest<DeleteProductImageCommandResponse>
	{
		public string Id { get; set; }
		public string? ImageId { get; set; }
	}
}
