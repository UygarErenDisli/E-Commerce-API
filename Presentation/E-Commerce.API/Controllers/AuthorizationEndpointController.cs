using E_Commerce.Application.Features.Commands.AuthorizationEndpoint.AssignRoleToEndpoint;
using E_Commerce.Application.Features.Quaries.AuthorizationEndpoint.GetRolesToEndpoint;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorizationEndpointController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthorizationEndpointController(IMediator mediator)
		{
			_mediator = mediator;
		}


		[HttpPost("[action]")]
		public async Task<IActionResult> GetRolesToEndpoint(GetRolesToEndpointQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> AssignRoleToEndpoint(AssignRoleToEndpointCommandRequest request)
		{
			request.Type = typeof(Program);
			var response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
