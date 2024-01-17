using E_Commerce.Application.Features.Commands.IdentityUser.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IdentityController : ControllerBase
	{
		private readonly IMediator _mediator;

		public IdentityController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}
	}
}
