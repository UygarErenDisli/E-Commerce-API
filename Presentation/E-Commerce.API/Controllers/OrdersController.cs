using E_Commerce.Application.Attributes;
using E_Commerce.Application.Consts;
using E_Commerce.Application.Features.Commands.Orders.CancelOrder;
using E_Commerce.Application.Features.Commands.Orders.CompleteOrder;
using E_Commerce.Application.Features.Commands.Orders.CreateOrder;
using E_Commerce.Application.Features.Quaries.Orders.GetAllOrders;
using E_Commerce.Application.Features.Quaries.Orders.GetByIdOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]
	public class OrdersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public OrdersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = Application.Enums.ActionType.Reading, Definition = "Get All Orders")]
		public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("{Id}")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = Application.Enums.ActionType.Reading, Definition = "Get Order By Id")]
		public async Task<IActionResult> GetOrderById([FromRoute] GetByIdOrderQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("[action]")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = Application.Enums.ActionType.Updating, Definition = "Complete Order")]
		public async Task<IActionResult> CompleteOrder(CompleteOrderCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}


		[HttpPost]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = Application.Enums.ActionType.Writing, Definition = "Create Order")]
		public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPut("[action]")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = Application.Enums.ActionType.Updating, Definition = "Cancel Order")]
		public async Task<IActionResult> CancelOrder(CancelOrderCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
