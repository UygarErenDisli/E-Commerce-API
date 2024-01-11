using E_Commerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.DeleteProductImage
{
	public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
	{
		private readonly IProductWriteRepository _productWriteRepository;

		public DeleteProductImageCommandHandler(IProductWriteRepository productWriteRepository)
		{
			_productWriteRepository = productWriteRepository;
		}

		public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
		{
			Domain.Entities.Product? productFromDb = await _productWriteRepository.Table.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

			var productImageFile = productFromDb?.ProductImages.FirstOrDefault(i => i.Id == Guid.Parse(input: request.ImageId!));

			if (productImageFile != null)
			{
				productFromDb?.ProductImages.Remove(productImageFile);
			}

			await _productWriteRepository.SaveAsync();

			return new();
		}
	}
}
