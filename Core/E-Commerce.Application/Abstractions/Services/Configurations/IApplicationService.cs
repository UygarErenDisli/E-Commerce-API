using E_Commerce.Application.DTOs.Configuration;

namespace E_Commerce.Application.Abstractions.Services.Configurations
{
	public interface IApplicationService
	{
		List<MenuDTO> GetAuthorizeDefinitionEndpoints(Type type);
	}
}
