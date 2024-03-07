using E_Commerce.Application.Abstractions.Services.Product;
using MediatR;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        private readonly IProductService _productService;

        public DeleteProductImageCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductImageAsync(request.Id, request.ImageId);

            return new();
        }
    }
}
