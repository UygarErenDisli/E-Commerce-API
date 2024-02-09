using E_Commerce.Application.Abstractions.Services.Identity;
using E_Commerce.Application.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace E_Commerce.API.Filters
{
	public class RolePermissionFilter : IAsyncActionFilter
	{
		private readonly IUserService _userService;

		public RolePermissionFilter(IUserService userService)
		{
			_userService = userService;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var name = context.HttpContext.User.Identity?.Name;
			if (!string.IsNullOrEmpty(name))
			{
				var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

				var authorizeDefinitionAttribute = descriptor!.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

				var httpAttribute = descriptor!.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

				var endpointCode = $"{httpAttribute?.HttpMethods.First() ?? HttpMethods.Get}.{authorizeDefinitionAttribute?.ActionType}.{authorizeDefinitionAttribute?.Definition.Replace(" ", "")}";

				var hasRole = await _userService.HasPermissionToEndpointAsync(name, endpointCode);
				if (!hasRole)
				{
					context.Result = new UnauthorizedResult();
				}
				else
				{
					await next();
				}
			}
			else
			{
				await next();
			}

		}
	}
}
