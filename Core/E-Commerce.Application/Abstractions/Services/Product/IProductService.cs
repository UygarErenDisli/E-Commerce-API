using E_Commerce.Application.DTOs.ProductImage;
using E_Commerce.Application.DTOs.Products;

namespace E_Commerce.Application.Abstractions.Services.Product
{
    public interface IProductService
    {
        Task<int> GetTotalOrdersCountAsync();
        List<ListProductsDTO> GetProducts(int pageIndex, int pageSize);
        Task<ProductDTO> GetProductByIdAsync(string id);
        Task<bool> CreateProductAsync(CreateProductDTO model);
        Task<bool> UpdateProductAsync(UpdateProductDTO model);
        Task<bool> DeleteProductByIdAsync(string id);
        Task<bool> UpdateProductShowcaseImageAsync(string productId, string imageId);
        Task<bool> DeleteProductImageAsync(string productId, string imageId);
        Task<bool> UploadProductImageAsync(string productId, List<ImageInfoDTO> images);
    }
}
