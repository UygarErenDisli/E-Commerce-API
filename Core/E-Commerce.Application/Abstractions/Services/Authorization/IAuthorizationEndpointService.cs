using E_Commerce.Application.DTOs.Role;

namespace E_Commerce.Application.Abstractions.Services.Authorization
{
	public interface IAuthorizationEndpointService
	{
		public Task AssignRoleToEndpoint(List<RoleDTO>? roles, string menuName, string endpointCode, Type type);

		public Task<List<RoleDTO>> GetRolesToEndpointAsync(string endpointCode, string menuName);
	}
}
