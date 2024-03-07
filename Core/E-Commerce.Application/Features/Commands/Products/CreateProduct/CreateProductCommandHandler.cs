using E_Commerce.Application.Abstractions.Hubs;
using E_Commerce.Application.Abstractions.Services.Product;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Products.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductHubService _productHubService;
        private readonly IProductService _productService;

        public CreateProductCommandHandler(IProductHubService productHubService, IProductService productService)
        {
            _productHubService = productHubService;
            _productService = productService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.CreateProductAsync(new()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            });
            await _productHubService.ProductAddedMessageAsync($"Product added: {request.Name}");
            return new();
        }
    }
}
