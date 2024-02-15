using E_Commerce.Application.DTOs.Products;

namespace E_Commerce.Application.Features.Quaries.Products.GetAllProducts
{
    public class GetAllProductsQueryResponse
	{
		public int TotalCount { get; set; }
		public List<ListProductsDTO> Products { get; set; }
	}
}
