using MediatR;

namespace E_Commerce.Application.Features.Quaries.Products.GetAllProducts
{
	public class GetAllProductsQueryRequest : IRequest<GetAllProductsQueryResponse>
	{
		public int PageIndex { get; set; } = 0;
		public int PageSize { get; set; } = 5;
	}
}
