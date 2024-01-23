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
		public async Task<IActionResult> GetBasketItems([FromQuery] GetAllBasketItemsQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateItemQuantity(UpdateItemQuantityCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpDelete("{BasketItemId}")]
		public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveBasketItemCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}


	}
}
