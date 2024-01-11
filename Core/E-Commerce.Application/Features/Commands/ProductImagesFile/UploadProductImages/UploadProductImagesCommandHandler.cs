using E_Commerce.Application.Abstractions;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.UploadProductImages
{
	public class UploadProductImagesCommandHandler : IRequestHandler<UploadProductImagesCommandRequest, UploadProductImagesCommandResponse>
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly IStorageService _storageService;
		private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

		public UploadProductImagesCommandHandler(IProductReadRepository productReadRepository, IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository)
		{
			_productReadRepository = productReadRepository;
			_storageService = storageService;
			_productImageFileWriteRepository = productImageFileWriteRepository;
		}

		public async Task<UploadProductImagesCommandResponse> Handle(UploadProductImagesCommandRequest request, CancellationToken cancellationToken)
		{
			var productFromDb = await _productReadRepository.GetByIdAsync(request.Id);

			var images = await _storageService.UploadAsync("product-images", request.Files);

			await _productImageFileWriteRepository.AddRangeAsync(images.Select(f => new ProductImageFile()
			{
				FileName = f.fileName,
				Path = f.path,
				Storage = _storageService.StorageName,
				Products = new List<Product>() { productFromDb }
			}).ToList());

			await _productImageFileWriteRepository.SaveAsync();

			return new();
		}
	}
}
