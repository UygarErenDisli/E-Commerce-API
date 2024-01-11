using E_Commerce.Application.Repositories;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.Products.GetByIdProduct
{
	public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
	{
		private readonly IProductReadRepository _productReadRepository;

		public GetByIdProductQueryHandler(IProductReadRepository productReadRepository)
		{
			_productReadRepository = productReadRepository;
		}

		public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
		{
			Domain.Entities.Product productFromDb = await _productReadRepository.GetByIdAsync(request.Id);

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
