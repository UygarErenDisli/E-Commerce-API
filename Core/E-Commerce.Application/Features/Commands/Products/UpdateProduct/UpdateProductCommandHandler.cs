using E_Commerce.Application.Abstractions.Services.Product;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Commands.Products.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IProductService _productService;
        private readonly ILogger<UpdateProductCommandHandler> _logger;
        public UpdateProductCommandHandler(IProductService productService, ILogger<UpdateProductCommandHandler> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateProductAsync(new()
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            });

            if (result)
            {
                _logger.LogInformation("Product updated id : {id}", request.Id);
            }
            return new();
        }
    }
}
