using E_Commerce.Application.Repositories;
using E_Commerce.Application.ViewModels.Products;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.Products.GetAllProducts
{
	public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
	{
		private readonly IProductReadRepository _productReadRepository;

		public GetAllProductsQueryHandler(IProductReadRepository productReadRepository)
		{
			_productReadRepository = productReadRepository;
		}

		public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
		{
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
