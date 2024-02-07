using E_Commerce.Application.Abstractions.Services;
using E_Commerce.Application.DTOs.Role;
using E_Commerce.Application.Exceptions;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Services
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<AppRole> _roleManager;

		public RoleService(RoleManager<AppRole> roleManager)
		{
			_roleManager = roleManager;
		}
		public async Task<ListRoleDTO> GetAllRolesAsync(int pageIndex, int pageSize)
		{
			var query = _roleManager.Roles;

			var roles = await query
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.Select(r => new RoleDTO()
				{
					Id = r.Id,
					Name = r.Name ?? "Role name was empty"
				}).ToListAsync();

			var totalCount = await query.CountAsync();

			return new()
			{
				Roles = roles,
				TotalRoleCount = totalCount
			};
		}

		public async Task<RoleDTO> GetRoleById(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role != null)
			{
				return new()
				{
					Id = role.Id,
					Name = role.Name ?? "Role name was empty"
				};
			}
			throw new RoleNotFoundException();
		}

		public async Task<bool> CreateRole(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				if (await _roleManager.FindByNameAsync(name) != null)
				{
					throw new ArgumentException("There is a role with the given name.", nameof(name));
				}
				var result = await _roleManager.CreateAsync(new() { Id = Guid.NewGuid().ToString(), Name = name });
				return result.Succeeded;
			}
			throw new ArgumentNullException(nameof(name), "Is not valid name for Role");
		}

		public async Task<bool> UpdateRole(string id, string newName)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role != null)
			{
				if (await _roleManager.FindByNameAsync(newName) != null)
				{
					throw new ArgumentException("There is a role with the given name.", nameof(newName));
				}
				role.Name = newName;
				var result = await _roleManager.UpdateAsync(role);
				return result.Succeeded;
			}
			throw new RoleNotFoundException();
		}

		public async Task<bool> DeleteRole(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role != null)
			{
				var result = await _roleManager.DeleteAsync(role);
				return result.Succeeded;
			}
			throw new RoleNotFoundException();

		}
	}
}
