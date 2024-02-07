using E_Commerce.Application.Attributes;
using E_Commerce.Application.Consts;
using E_Commerce.Application.Features.Commands.ProductImagesFile.ChangeImageToShowcase;
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
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = Application.Enums.ActionType.Writing, Definition = "Create Product")]
		public async Task<IActionResult> Post(CreateProductCommandRequest request)
		{
			var _ = await _mediator.Send(request);

			return Ok((int)HttpStatusCode.Created);
		}

		[HttpPut]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = Application.Enums.ActionType.Updating, Definition = "Updating Product")]
		public async Task<IActionResult> Put(UpdateProductCommandRequest request)
		{
			var _ = await _mediator.Send(request);

			return Ok();
		}

		[HttpDelete("{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = Application.Enums.ActionType.Deleting, Definition = "Deleting Product")]
		public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest request)
		{
			var _ = await _mediator.Send(request);

			return Ok();
		}


		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = Application.Enums.ActionType.Writing, Definition = "Upload Product Image Files")]
		public async Task<IActionResult> UploadProductImages([FromQuery] UploadProductImagesCommandRequest request)
		{
			request.Files = Request.Form.Files;

			var _ = await _mediator.Send(request);

			return Ok();
		}

		[HttpGet("[action]/{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = Application.Enums.ActionType.Reading, Definition = "Get Product Images")]
		public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesByProductIdQueryRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}

		[HttpDelete("[action]/{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = Application.Enums.ActionType.Deleting, Definition = "Deleting Product Image File")]
		public async Task<IActionResult> DeleteProductImage([FromRoute] DeleteProductImageCommandRequest request, [FromQuery] string imageId)
		{

			request.ImageId = imageId;

			var _ = await _mediator.Send(request);

			return Ok();
		}

		[HttpPut("[action]")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = Application.Enums.ActionType.Updating, Definition = "Update Product Showcase Image File")]
		public async Task<IActionResult> ChangeImageToShowCase([FromBody] ChangeImageToShowcaseCommandRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}
	}
}
