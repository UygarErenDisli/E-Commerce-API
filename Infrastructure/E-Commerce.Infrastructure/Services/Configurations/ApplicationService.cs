using E_Commerce.Application.Abstractions.Services.Configurations;
using E_Commerce.Application.Attributes;
using E_Commerce.Application.DTOs.Configuration;
using E_Commerce.Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using Action = E_Commerce.Application.DTOs.Configuration.Action;

namespace E_Commerce.Infrastructure.Services.Configurations
{
	public class ApplicationService : IApplicationService
	{
		public List<MenuDTO> GetAuthorizeDefinitionEndpoints(Type type)
		{
			var currentAssembly = Assembly.GetAssembly(type) ?? throw new Exception("An Error Occured");
			var controllers = currentAssembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));
			List<MenuDTO> output = [];
			if (controllers != null)
			{
				foreach (var controller in controllers)
				{
					var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));

					if (actions != null)
					{
						foreach (var action in actions)
						{
							var attributes = action.GetCustomAttributes(true);
							if (attributes != null)
							{
								MenuDTO? menu = null;
								var attribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

								if (!output.Any(m => m.Name == attribute!.Menu))
								{
									menu = new() { Name = attribute!.Menu };
									output.Add(menu);
								}
								else
								{
									menu = output.FirstOrDefault(m => m.Name == attribute!.Menu);
								}
								Action actionToBeAdded = new()
								{
									ActionType = Enum.GetName(typeof(ActionType), attribute!.ActionType)!,
									Definition = attribute.Definition
								};

								var httpMethod = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
								if (httpMethod != null)
								{
									actionToBeAdded.HttpType = httpMethod.HttpMethods.First();
								}
								else
								{
									actionToBeAdded.HttpType = HttpMethods.Get;
								}
								actionToBeAdded.ActionCode = $"{actionToBeAdded.HttpType}.{actionToBeAdded.ActionType}.{actionToBeAdded.Definition.Replace(" ", "")}";
								menu!.Actions.Add(actionToBeAdded);
							}
						}
					}
				}
			}
			return output;
		}
	}
}
