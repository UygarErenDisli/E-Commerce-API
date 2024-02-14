using E_Commerce.Application.DTOs.Role;
using E_Commerce.Application.DTOs.User;
using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Application.Abstractions.Services.Identity
{
	public interface IUserService
	{
		Task<CreateUserResponse> CreateUser(CreateUser model);
		Task UpdateRefreshToken(string refreshToken, AppUser? user, DateTime accessTokenDate, int addOnAccessTokenInMinutes);
		Task<ListUsersDTO> GetAllUsers(int pageIndex, int pageSize);
		Task AssignRolesToUser(string userId, List<RoleDTO>? roles);
		Task AssignRoleToUser(string userId, RoleDTO roles);
		Task<string[]> GetRolesToUserAsync(string userId);
		Task<bool> HasPermissionToEndpointAsync(string userNameOrId, string endpointCode);
	}
}
