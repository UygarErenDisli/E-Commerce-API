using E_Commerce.Application.Abstractions.Services.Product;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.Products.GetByIdProduct
{
	public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
	{
		private readonly IProductService _productService;

		public GetByIdProductQueryHandler(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
		{
			var productFromDb = await _productService.GetProductByIdAsync(request.Id);

			GetByIdProductQueryResponse response = new()
			{
				Name = productFromDb.Name,
				Price = productFromDb.Price,
				Stock = productFromDb.Stock
			};

			return response;
		}
	}
}
