using E_Commerce.Application.Abstractions;
using E_Commerce.Application.Repositories;
using E_Commerce.Application.RequiestParameters;
using E_Commerce.Application.ViewModels.Products;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly IProductWriteRepository _productWriteRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IStorageService _storageService;
		private readonly IFileReadRepository _fileReadRepository;
		private readonly IFileWriteRepository _fileWriteRepository;
		private readonly IProductImageFileReadRepository _productImageFileReadRepository;
		private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
		private readonly IConfiguration _configuration;

		public ProductsController(
			IProductReadRepository productReadRepository,
			IProductWriteRepository productWriteRepository,
			IWebHostEnvironment webHostEnvironment,
			IStorageService storageService,
			IFileReadRepository fileReadRepository,
			IFileWriteRepository fileWriteRepository,
			IProductImageFileReadRepository productImageFileReadRepository,
			IProductImageFileWriteRepository productImageFileWriteRepository,
			IConfiguration configuration)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
			_webHostEnvironment = webHostEnvironment;
			_storageService = storageService;
			_fileReadRepository = fileReadRepository;
			_fileWriteRepository = fileWriteRepository;
			_productImageFileReadRepository = productImageFileReadRepository;
			_productImageFileWriteRepository = productImageFileWriteRepository;
			_configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] Pagination pagination)
		{
			var totalCount = _productReadRepository.GetAll().Count();
			var products = _productReadRepository.GetAll(false).Select(p => new ListProductsDTO
			{
				Id = $"{p.Id}",
				Name = p.Name,
				Stock = p.Stock,
				Price = p.Price,
				CreatedDate = p.CreatedDate,
				UpdatedDate = p.UpdatedDate

			}).Skip((pagination.PageIndex * pagination.PageSize)).Take(pagination.PageSize).ToList();

			return Ok(new ListProductVM() { TotalCount = totalCount, Products = products });
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			return Ok(await _productReadRepository.GetByIdAsync(id));
		}

		[HttpPost]
		public async Task<IActionResult> Post(CreateProductVM model)
		{
			Product product = new()
			{
				Name = model.Name,
				Stock = model.Stock,
				Price = model.Price
			};
			await _productWriteRepository.AddAsync(product);
			await _productWriteRepository.SaveAsync();
			return Ok((int)HttpStatusCode.Created);
		}

		[HttpPut]
		public async Task<IActionResult> Put(UpdateProductVM model)
		{
			Product product = await _productReadRepository.GetByIdAsync(model.Id);

			product.Name = model.Name;
			product.Stock = model.Stock;
			product.Price = model.Price;

			_productWriteRepository.Update(product);
			await _productWriteRepository.SaveAsync();

			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			Product product = await _productReadRepository.GetByIdAsync(id);

			_productWriteRepository.Remove(product);
			await _productWriteRepository.SaveAsync();
			return Ok();
		}


		[HttpPost("[action]")]
		public async Task<IActionResult> Upload([FromQuery] string id)
		{
			var productFromDb = await _productReadRepository.GetByIdAsync(id);

			var datas = await _storageService.UploadAsync("product-images", Request.Form.Files);

			await _productImageFileWriteRepository.AddRangeAsync(datas.Select(f => new ProductImageFile()
			{
				FileName = f.fileName,
				Path = f.path,
				Storage = _storageService.StorageName,
				Products = new List<Product>() { productFromDb }
			}).ToList());

			await _productImageFileWriteRepository.SaveAsync();
			return Ok();
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetProductImages(string id)
		{
			var productFromDb = await _productWriteRepository.Table.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

			return Ok(productFromDb.ProductImages.Select(i => new
			{
				Path = $"{_configuration["StorageBaseUrl"]}/{i.Path}",
				i.Id,
				i.FileName
			}));
		}

		[HttpDelete("[action]/{id}")]
		public async Task<IActionResult> DeleteProductImage(string id, string imageId)
		{

			var productFromDb = await _productWriteRepository.Table.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

			var productImageFile = productFromDb.ProductImages.FirstOrDefault(i => i.Id == Guid.Parse(imageId));

			productFromDb.ProductImages.Remove(productImageFile);

			await _productWriteRepository.SaveAsync();

			return Ok();
		}
	}
}
