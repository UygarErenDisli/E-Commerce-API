using E_Commerce.Application.Attributes;
using E_Commerce.Application.Consts;
using E_Commerce.Application.Features.Commands.Roles.CreateRole;
using E_Commerce.Application.Features.Commands.Roles.DeleteRole;
using E_Commerce.Application.Features.Commands.Roles.UpdateRole;
using E_Commerce.Application.Features.Quaries.Roles.GetAllRoles;
using E_Commerce.Application.Features.Quaries.Roles.GetRoleById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]
	public class RolesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public RolesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[AuthorizeDefinition(
			Menu = AuthorizeDefinitionConstants.Roles,
			ActionType = Application.Enums.ActionType.Reading,
			Definition = "Get All Roles With Pagination")
		]
		public async Task<IActionResult> GetAllRoles([FromQuery] GetAllRolesQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("{Id}")]
		[AuthorizeDefinition(
			Menu = AuthorizeDefinitionConstants.Roles,
			ActionType = Application.Enums.ActionType.Reading,
			Definition = "Get Role With Id")
		]
		public async Task<IActionResult> GetRoleById([FromRoute] GetRoleByIdQueryRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost]
		[AuthorizeDefinition(
			Menu = AuthorizeDefinitionConstants.Roles,
			ActionType = Application.Enums.ActionType.Writing,
			Definition = "Create Role")
		]
		public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPut]
		[AuthorizeDefinition(
			Menu = AuthorizeDefinitionConstants.Roles,
			ActionType = Application.Enums.ActionType.Updating,
			Definition = "Update Role")
		]
		public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpDelete("{Id}")]
		[AuthorizeDefinition(
			Menu = AuthorizeDefinitionConstants.Roles,
			ActionType = Application.Enums.ActionType.Deleting,
			Definition = "Delete Role")
		]
		public async Task<IActionResult> DeleteRole([FromRoute] DeleteRoleCommandRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
