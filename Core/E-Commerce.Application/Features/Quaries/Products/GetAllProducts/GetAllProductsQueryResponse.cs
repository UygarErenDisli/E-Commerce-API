using E_Commerce.Application.ViewModels.Products;

namespace E_Commerce.Application.Features.Quaries.Products.GetAllProducts
{
	public class GetAllProductsQueryResponse
	{
		public int TotalCount { get; set; }
		public List<ListProductsDTO> Products { get; set; }
	}
}
