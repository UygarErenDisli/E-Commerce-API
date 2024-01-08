using E_Commerce.Application.Abstractions;
using E_Commerce.Application.Repositories;
using E_Commerce.Application.RequiestParameters;
using E_Commerce.Application.ViewModels.Products;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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

		public ProductsController(
			IProductReadRepository productReadRepository,
			IProductWriteRepository productWriteRepository,
			IWebHostEnvironment webHostEnvironment,
			IStorageService storageService,
			IFileReadRepository fileReadRepository,
			IFileWriteRepository fileWriteRepository,
			IProductImageFileReadRepository productImageFileReadRepository,
			IProductImageFileWriteRepository productImageFileWriteRepository)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
			_webHostEnvironment = webHostEnvironment;
			_storageService = storageService;
			_fileReadRepository = fileReadRepository;
			_fileWriteRepository = fileWriteRepository;
			_productImageFileReadRepository = productImageFileReadRepository;
			_productImageFileWriteRepository = productImageFileWriteRepository;
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
		public async Task<IActionResult> Upload()
		{
			var datas = await _storageService.UploadAsync("files", Request.Form.Files);

			await _productImageFileWriteRepository.AddRangeAsync(datas.Select(f => new ProductImageFile()
			{
				FileName = f.fileName,
				Path = f.path,
				Storage = _storageService.StorageName,
			}).ToList());

			await _productImageFileWriteRepository.SaveAsync();
			return Ok();
		}
	}
}
