using E_Commerce.Application.Features.Commands.IdentityUser.CreateUser;
using E_Commerce.Application.Features.Commands.IdentityUser.DeleteNotification;
using E_Commerce.Application.Features.Quaries.IdentityUser.GetUserNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

		[HttpGet]
		[Authorize(AuthenticationSchemes = "Admin")]
		public async Task<IActionResult> GetUserNotifications([FromQuery] GetUserNotificationsQueryRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}

		[HttpDelete("{Id}")]
		public async Task<IActionResult> DeleteNotification([FromRoute] DeleteNotificationCommandRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}
	}

}
