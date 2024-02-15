using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.DTOs.Products
{
    public class ListProductDTO
    {
        public int TotalCount { get; set; }
        public List<ListProductsDTO> Products { get; set; }
    }

    public class ListProductsDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<ProductImageFile> ProductImages { get; set; }

    }
}


