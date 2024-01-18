using E_Commerce.Application.Features.Commands.ProductImagesFile.DeleteProductImage;
using E_Commerce.Application.Features.Commands.ProductImagesFile.UploadProductImages;
using E_Commerce.Application.Features.Commands.Products.CreateProduct;
using E_Commerce.Application.Features.Commands.Products.DeleteProduct;
using E_Commerce.Application.Features.Commands.Products.UpdateProduct;
using E_Commerce.Application.Features.Quaries.ProductImagesFile.GetProductImages;
using E_Commerce.Application.Features.Quaries.Products.GetAllProducts;
using E_Commerce.Application.Features.Quaries.Products.GetByIdProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]
	public class ProductsController : ControllerBase
	{

		private readonly IMediator _mediator;

		public ProductsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Post(CreateProductCommandRequest request)
		{
			var _ = await _mediator.Send(request);

			return Ok((int)HttpStatusCode.Created);
		}

		[HttpPut]
		public async Task<IActionResult> Put(UpdateProductCommandRequest request)
		{
			var _ = await _mediator.Send(request);

			return Ok();
		}

		[HttpDelete("{Id}")]
		public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest request)
		{
			var _ = await _mediator.Send(request);

			return Ok();
		}


		[HttpPost("[action]")]
		public async Task<IActionResult> UploadProductImages([FromQuery] UploadProductImagesCommandRequest request)
		{
			request.Files = Request.Form.Files;

			var _ = await _mediator.Send(request);

			return Ok();
		}

		[HttpGet("[action]/{Id}")]
		public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesByProductIdQueryRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}

		[HttpDelete("[action]/{Id}")]
		public async Task<IActionResult> DeleteProductImage([FromRoute] DeleteProductImageCommandRequest request, [FromQuery] string imageId)
		{

			request.ImageId = imageId;

			var _ = await _mediator.Send(request);

			return Ok();
		}
	}
}
