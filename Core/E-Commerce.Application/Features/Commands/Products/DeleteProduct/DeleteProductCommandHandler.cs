using E_Commerce.Application.Abstractions.Services.Product;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Commands.Products.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        private readonly IProductService _productService;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(IProductService productService, ILogger<DeleteProductCommandHandler> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.DeleteProductByIdAsync(request.Id);
            if (result)
            {
                _logger.LogInformation("Product deleted Id: {Id}", request.Id);
            }
            return new();
        }
    }
}
