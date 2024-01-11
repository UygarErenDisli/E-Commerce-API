using MediatR;

namespace E_Commerce.Application.Features.Quaries.ProductImagesFile.GetProductImages
{
	public class GetProductImagesByProductIdQueryRequest : IRequest<List<GetProductImagesByProductIdQueryResponse>>
	{
		public string Id { get; set; }
	}
}
