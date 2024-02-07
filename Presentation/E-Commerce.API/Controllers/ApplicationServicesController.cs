using E_Commerce.Application.Abstractions.Services.Configurations;
using E_Commerce.Application.Attributes;
using E_Commerce.Application.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Admin")]
	public class ApplicationServicesController : ControllerBase
	{
		private readonly IApplicationService _applicationService;

		public ApplicationServicesController(IApplicationService applicationService)
		{
			_applicationService = applicationService;
		}

		[HttpGet]
		[AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.ApplicationService, ActionType = Application.Enums.ActionType.Reading, Definition = "Get Authorize Definition Endpoints")]
		public IActionResult GetAuthorizeDefinitionEndpoints()
		{
			var response = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));

			return Ok(response);
		}
	}
}
