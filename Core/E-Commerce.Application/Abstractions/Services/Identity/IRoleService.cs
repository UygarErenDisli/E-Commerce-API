﻿using E_Commerce.Application.DTOs.Role;

namespace E_Commerce.Application.Abstractions.Services.Identity
{
	public interface IRoleService
	{
		Task<ListRoleDTO> GetAllRolesAsync(int pageIndex, int pageSize);
		Task<RoleDTO> GetRoleById(string id);
		Task<RoleDTO> GetRoleByName(string roleName);
		Task<bool> CreateRole(string name);
		Task<bool> UpdateRole(string id, string newName);
		Task<bool> DeleteRole(string id);
	}
}
