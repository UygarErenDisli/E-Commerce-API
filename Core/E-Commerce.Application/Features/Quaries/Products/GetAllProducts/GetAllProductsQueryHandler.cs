using E_Commerce.Application.Abstractions.Services.Product;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Quaries.Products.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        private readonly IProductService _productService;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IProductService productService, ILogger<GetAllProductsQueryHandler> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Requested products count: {ProductCount}", request.PageSize);

            var totalCount = await _productService.GetTotalOrdersCountAsync();
            var products = _productService.GetProducts(request.PageIndex, request.PageSize);

            return new() { Products = products, TotalCount = totalCount };
        }
    }
}
