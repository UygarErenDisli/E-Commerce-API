using E_Commerce.Application.Attributes;
using E_Commerce.Application.Consts;
using E_Commerce.Application.Features.Commands.Baskets.AddItemToBasket;
using E_Commerce.Application.Features.Commands.Baskets.RemoveBasketItem;
using E_Commerce.Application.Features.Commands.Baskets.UpdateItemQuantity;
using E_Commerce.Application.Features.Quaries.Baskets.GetAllBasketItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]
	public class BasketsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BasketsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = Application.Enums.ActionType.Reading, Definition = "Get Basket Items")]
		public async Task<IActionResult> GetBasketItems([FromQuery] GetAllBasketItemsQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = Application.Enums.ActionType.Writing, Definition = "Add Item To Basket")]
		public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPut]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = Application.Enums.ActionType.Updating, Definition = "Update Item Quantity")]
		public async Task<IActionResult> UpdateItemQuantity(UpdateItemQuantityCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpDelete("{BasketItemId}")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = Application.Enums.ActionType.Deleting, Definition = "Deleting Basket Item")]
		public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveBasketItemCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
