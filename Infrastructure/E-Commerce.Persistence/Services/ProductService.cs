using E_Commerce.Application.Abstractions.Services.Product;
using E_Commerce.Application.DTOs.ProductImage;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Exceptions;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ProductService(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<int> GetTotalOrdersCountAsync() => await _productReadRepository.Table.CountAsync();

        public List<ListProductsDTO> GetProducts(int pageIndex, int pageSize)
        {
            var orderedProducts = _productReadRepository
            .GetAll(false)
            .OrderBy(p => p.CreatedDate);

            var output = orderedProducts
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(p => new ListProductsDTO
            {
                Id = $"{p.Id}",
                Name = p.Name,
                Stock = p.Stock,
                Price = p.Price,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate,
                ProductImages = p.ProductImages
            });
            return [.. output];
        }

        public async Task<ProductDTO> GetProductByIdAsync(string id)
        {
            ThrowArgumentExceptionIfNullOrEmpty(id);

            var product = await _productReadRepository.GetByIdAsync(id)
               ??
               throw new ProductNotFoundException();

            return new()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public async Task<bool> CreateProductAsync(CreateProductDTO model)
        {
            if (model is null)
            {
                throw new ArgumentException($"Given model is null {nameof(model)}");
            }

            var result = await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
            });

            if (!result)
            {
                return result;
            }
            await _productWriteRepository.SaveAsync();
            return result;
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDTO model)
        {
            if (model is null)
            {
                throw new ArgumentException($"Given model is null {nameof(model)}");
            }

            var product = await _productReadRepository.GetByIdAsync(model.Id)
                ?? throw new ProductNotFoundException();

            product.Name = model.Name;
            product.Stock = model.Stock;
            product.Price = model.Price;

            var result = _productWriteRepository.Update(product);

            if (result)
            {
                await _productWriteRepository.SaveAsync();
                return result;
            }
            return result;
        }

        public async Task<bool> DeleteProductByIdAsync(string id)
        {
            ThrowArgumentExceptionIfNullOrEmpty(id);

            Product product = await _productReadRepository.GetByIdAsync(id)
                ??
                throw new ProductNotFoundException();

            var result = _productWriteRepository.Remove(product);
            if (!result)
            {
                return false;
            }
            await _productWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateProductShowcaseImageAsync(string productId, string imageId)
        {
            ThrowArgumentExceptionIfNullOrEmpty(productId);
            ThrowArgumentExceptionIfNullOrEmpty(imageId);

            var query = _productReadRepository.Table.Include(p => p.ProductImages);

            var product = await query
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId))
                ??
                throw new ProductNotFoundException();

            var oldShowcaseImage = product.ProductImages.
                FirstOrDefault(i => i.IsShowCaseImage == true);
            if (oldShowcaseImage != null)
            {
                oldShowcaseImage.IsShowCaseImage = false;
            }

            var image = product.ProductImages
                .FirstOrDefault(i => i.Id == Guid.Parse(imageId))
                ??
                throw new ProductImageNotFoundException();

            image!.IsShowCaseImage = true;

            await _productWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteProductImageAsync(string productId, string imageId)
        {
            ThrowArgumentExceptionIfNullOrEmpty(productId);
            ThrowArgumentExceptionIfNullOrEmpty(imageId);

            var product = await _productReadRepository.Table
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId))
                ??
                throw new ProductNotFoundException();

            var productImage = product.ProductImages.FirstOrDefault(i => i.Id == Guid.Parse(imageId))
                ??
                throw new ProductImageNotFoundException();

            var result = product.ProductImages.Remove(productImage);

            if (result)
            {
                await _productWriteRepository.SaveAsync();
                return result;
            }
            return result;
        }

        public async Task<bool> UploadProductImageAsync(string productId, List<ImageInfoDTO> images)
        {
            ThrowArgumentExceptionIfNullOrEmpty(productId);

            var product = await _productReadRepository.GetByIdAsync(productId)
                ??
                throw new ProductNotFoundException();

            var result = await _productImageFileWriteRepository.AddRangeAsync(images.Select(i => new ProductImageFile()
            {
                FileName = i.FileName,
                Path = i.Path,
                Storage = i.StorageServiceName,
                Products = [product]
            }).ToList());

            if (result)
            {
                await _productImageFileWriteRepository.SaveAsync();
                return result;
            }
            return result;
        }

        private static void ThrowArgumentExceptionIfNullOrEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Given value can not be null or empty: {Value}", value);
            }
        }
    }
}
