using E_Commerce.Application.Repositories;
using E_Commerce.Application.ViewModels.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Application.Features.Quaries.Products.GetAllProducts
{
	public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly ILogger<GetAllProductsQueryHandler> _logger;
		public GetAllProductsQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductsQueryHandler> logger)
		{
			_productReadRepository = productReadRepository;
			_logger = logger;
		}

		public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Requested products count: {ProductCount}", request.PageSize);
			var totalCount = _productReadRepository.GetAll().Count();
			List<ListProductsDTO> products =
			[
				.. _productReadRepository.GetAll(false).Select(p => new ListProductsDTO
				{
					Id = $"{p.Id}",
					Name = p.Name,
					Stock = p.Stock,
					Price = p.Price,
					CreatedDate = p.CreatedDate,
					UpdatedDate = p.UpdatedDate

				}).Skip((request.PageIndex * request.PageSize)).Take(request.PageSize),
			];

			return new() { Products = products, TotalCount = totalCount };
		}
	}
}
