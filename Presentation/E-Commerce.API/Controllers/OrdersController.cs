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
		public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetOrderById([FromRoute] GetByIdOrderQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> CompleteOrder(CompleteOrderCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}


		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
