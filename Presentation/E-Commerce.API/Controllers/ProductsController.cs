﻿using E_Commerce.Application.Repositories;
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

		public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(_productReadRepository.GetAll(false));
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
	}
}