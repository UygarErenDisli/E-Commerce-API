using E_Commerce.Application.Abstractions.Services.Product;
using MediatR;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.ChangeImageToShowcase
{
    public class ChangeImageToShowcaseCommandHandler : IRequestHandler<ChangeImageToShowcaseCommandRequest, ChangeImageToShowcaseCommandResponse>
    {
        private readonly IProductService _productService;

        public ChangeImageToShowcaseCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ChangeImageToShowcaseCommandResponse> Handle(ChangeImageToShowcaseCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.UpdateProductShowcaseImageAsync(request.ProductId, request.ImageId);

            return new();

        }
    }
}
