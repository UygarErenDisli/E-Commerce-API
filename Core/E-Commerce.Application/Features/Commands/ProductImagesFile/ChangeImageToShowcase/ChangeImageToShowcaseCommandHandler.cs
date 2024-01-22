using E_Commerce.Application.Exceptions;
using E_Commerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.ChangeImageToShowcase
{
	public class ChangeImageToShowcaseCommandHandler : IRequestHandler<ChangeImageToShowcaseCommandRequest, ChangeImageToShowcaseCommandResponse>
	{
		private readonly IProductWriteRepository _productWriteRepository;

		public ChangeImageToShowcaseCommandHandler(IProductWriteRepository productWriteRepository)
		{
			_productWriteRepository = productWriteRepository;
		}

		public async Task<ChangeImageToShowcaseCommandResponse> Handle(ChangeImageToShowcaseCommandRequest request, CancellationToken cancellationToken)
		{
			var product = await _productWriteRepository.Table.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.ProductId));

			if (product == null)
			{
				throw new ProductNotFoundException();
			}

			var lastShowcaseImage = product.ProductImages.FirstOrDefault(i => i.IsShowCaseImage == true);
			if (lastShowcaseImage != null)
			{
				lastShowcaseImage.IsShowCaseImage = false;
			}

			var image = product.ProductImages.FirstOrDefault(i => i.Id == Guid.Parse(request.ImageId));
			if (image == null)
			{
				throw new ProductImageNotFoundException();
			}

			image!.IsShowCaseImage = true;

			await _productWriteRepository.SaveAsync();

			return new();

		}
	}
}
