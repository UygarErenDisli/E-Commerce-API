using E_Commerce.Application.Attributes;
using E_Commerce.Application.Consts;
using E_Commerce.Application.Features.Commands.IdentityUser.AssignRolesToUser;
using E_Commerce.Application.Features.Commands.IdentityUser.CreateUser;
using E_Commerce.Application.Features.Commands.IdentityUser.DeleteNotification;
using E_Commerce.Application.Features.Quaries.IdentityUser.GetAllUsers;
using E_Commerce.Application.Features.Quaries.IdentityUser.GetRolesToUserAsync;
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


		[HttpGet("[action]")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Identity, ActionType = Application.Enums.ActionType.Reading, Definition = "Get All Users")]
		public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("[action]/{UserId}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Identity, ActionType = Application.Enums.ActionType.Reading, Definition = "Get Roles To User")]
		public async Task<IActionResult> GetRolesToUser([FromRoute] GetRolesToUserQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Identity, ActionType = Application.Enums.ActionType.Updating, Definition = "Assign Roles To User")]
		public async Task<IActionResult> AssignRolesToUser(AssignRolesToUserCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}


		[HttpGet]
		public async Task<IActionResult> GetUserNotifications([FromQuery] GetUserNotificationsQueryRequest request)
		{
			var response = await _mediator.Send(request);

			return Ok(response);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
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
