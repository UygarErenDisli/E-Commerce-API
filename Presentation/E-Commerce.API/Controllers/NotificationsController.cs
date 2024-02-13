using E_Commerce.Application.Attributes;
using E_Commerce.Application.Consts;
using E_Commerce.Application.Features.Commands.IdentityUser.DeleteNotification;
using E_Commerce.Application.Features.Quaries.IdentityUser.GetUserNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public NotificationsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Notifications, ActionType = Application.Enums.ActionType.Reading, Definition = "Get User Notifications")]
		public async Task<IActionResult> GetUserNotifications([FromQuery] GetUserNotificationsQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpDelete("{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Notifications, ActionType = Application.Enums.ActionType.Deleting, Definition = "Delete Notification")]
		public async Task<IActionResult> DeleteNotification([FromRoute] DeleteNotificationCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
