using E_Commerce.Application.Abstractions.Services.Authorization;
using E_Commerce.Application.Abstractions.Services.Configurations;
using E_Commerce.Application.DTOs.Role;
using E_Commerce.Application.Exceptions;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Services
{
	public class AuthorizationEndpointService : IAuthorizationEndpointService
	{
		private readonly IApplicationService _applicationService;
		private readonly IEndpointWriteRepository _endpointWriteRepository;
		private readonly IEndpointReadRepository _endpointReadRepository;
		private readonly IMenuReadRepository _menuReadRepository;
		private readonly IMenuWriteRepository _menuWriteRepository;
		private readonly RoleManager<AppRole> _roleManager;

		public AuthorizationEndpointService(IApplicationService applicationService, IEndpointWriteRepository endpointWriteRepository, IEndpointReadRepository endpointReadRepository, IMenuReadRepository menuReadRepository, IMenuWriteRepository menuWriteRepository, RoleManager<AppRole> roleManager)
		{
			_applicationService = applicationService;
			_endpointWriteRepository = endpointWriteRepository;
			_endpointReadRepository = endpointReadRepository;
			_menuReadRepository = menuReadRepository;
			_menuWriteRepository = menuWriteRepository;
			_roleManager = roleManager;
		}

		public async Task AssignRoleToEndpoint(List<RoleDTO>? roles, string menuName, string endpointCode, Type type)
		{
			var menu =
				await _menuReadRepository.GetSingleAsync(m => m.Name == menuName)
				??
				await CreateMenu(menuName);

			var endpoint = await _endpointReadRepository.Table
				.Include(e => e.Menu)
				.Include(e => e.Roles)
				.FirstOrDefaultAsync(e => e.Code == endpointCode &&
				e.Menu.Name == menuName);

			if (endpoint == null)
			{
				var action = _applicationService.GetAuthorizeDefinitionEndpoints(type)
					.FirstOrDefault(m => m.Name == menuName)?.Actions
					.FirstOrDefault(a => a.ActionCode == endpointCode)
					??
					throw new ActionNotFoundException();

				endpoint = await CreateEndpoint(endpointCode, menu, action);
			}
			RemoveOldRoles(endpoint);
			await AddRolesToEndpoint(roles, endpoint);
			await _endpointWriteRepository.SaveAsync();
		}
		public async Task<List<RoleDTO>> GetRolesToEndpointAsync(string endpointCode, string menuName)
		{
			var endpoint = await _endpointReadRepository.Table
				.Include(e => e.Menu)
				.Include(e => e.Roles)
				.FirstOrDefaultAsync(e => e.Code == endpointCode && e.Menu.Name == menuName);

			if (endpoint != null)
			{
				return endpoint.Roles.Select(e => new RoleDTO()
				{
					Id = e.Id,
					Name = e.Name!
				}).ToList();
			}
			return [];
		}
		private async Task<Menu> CreateMenu(string menuName)
		{
			Menu menu = new()
			{
				Id = Guid.NewGuid(),
				Name = menuName
			};
			await _menuWriteRepository.AddAsync(menu);
			await _menuWriteRepository.SaveAsync();
			return menu;
		}
		private async Task<Endpoint> CreateEndpoint(string endpointCode, Menu menu, Application.DTOs.Configuration.Action action)
		{
			Endpoint endpoint = new()
			{
				Id = Guid.NewGuid(),
				Code = endpointCode,
				Menu = menu,
				ActionType = action.ActionType,
				Definition = action.Definition,
				HttpType = action.HttpType,
			};
			await _endpointWriteRepository.AddAsync(endpoint);
			await _endpointWriteRepository.SaveAsync();
			return endpoint;
		}
		private async Task AddRolesToEndpoint(List<RoleDTO>? roles, Endpoint endpoint)
		{
			if (roles != null)
			{
				var mappedRoles = new List<AppRole>();
				foreach (var role in roles)
				{
					var roleFromDb = await _roleManager.FindByIdAsync(role.Id);
					if (roleFromDb != null)
					{
						mappedRoles.Add(roleFromDb);
					}
				}

				foreach (var mappedRole in mappedRoles)
				{
					endpoint.Roles.Add(mappedRole);
				}
			}
		}
		private void RemoveOldRoles(Endpoint endpoint)
		{
			foreach (var role in endpoint.Roles)
			{
				endpoint.Roles.Remove(role);
			}
		}

	}
}
