using E_Commerce.Application.Abstractions.Services.Product;
using E_Commerce.Application.Abstractions.Storage;
using MediatR;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.UploadProductImages
{
    public class UploadProductImagesCommandHandler : IRequestHandler<UploadProductImagesCommandRequest, UploadProductImagesCommandResponse>
    {
        private readonly IProductService _productService;
        private readonly IStorageService _storageService;

        public UploadProductImagesCommandHandler(IProductService productService, IStorageService storageService)
        {
            _productService = productService;
            _storageService = storageService;
        }

        public async Task<UploadProductImagesCommandResponse> Handle(UploadProductImagesCommandRequest request, CancellationToken cancellationToken)
        {
            var images = await _storageService.UploadAsync("product-images", request.Files);

            await _productService.UploadProductImageAsync(request.Id, images);

            return new();
        }
    }
}
