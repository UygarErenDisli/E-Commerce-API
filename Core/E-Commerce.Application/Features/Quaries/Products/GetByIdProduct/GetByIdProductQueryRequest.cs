using MediatR;

namespace E_Commerce.Application.Features.Quaries.Products.GetByIdProduct
{
	public class GetByIdProductQueryRequest : IRequest<GetByIdProductQueryResponse>
	{
		public string Id { get; set; }
	}
}
