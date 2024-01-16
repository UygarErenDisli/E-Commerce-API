using E_Commerce.Application.Features.Commands.IdentityUser.CreateUser;
using E_Commerce.Application.Features.Commands.IdentityUser.GoogleLogin;
using E_Commerce.Application.Features.Commands.IdentityUser.LoginUser;
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

		[HttpPost("[action]")]
		public async Task<IActionResult> Login(LoginUserCommandRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}
		[HttpPost("[action]")]
		public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}
	}
}
