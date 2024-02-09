using E_Commerce.Application.Abstractions.Services.Identity;
using E_Commerce.Application.DTOs.Role;
using E_Commerce.Application.DTOs.User;
using E_Commerce.Application.Exceptions;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IEndpointReadRepository _endpointReadRepository;

		public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository)
		{
			_userManager = userManager;
			_endpointReadRepository = endpointReadRepository;
		}

		public async Task<ListUsersDTO> GetAllUsers(int pageIndex, int pageSize)
		{
			var query = _userManager.Users;

			var totalCount = await query.CountAsync();
			var users = await query
				.Skip(pageIndex * pageSize).Take(pageSize)
				.Select(u => new ListUserDTO()
				{
					Email = u.Email!,
					NameSurname = u.NameSurname,
					UserId = u.Id!,
					UserName = u.UserName!
				})
				.ToListAsync();

			return new()
			{
				TotalCount = totalCount,
				Users = users
			};

		}

		public async Task<CreateUserResponse> CreateUser(CreateUser model)
		{
			var result = await _userManager.CreateAsync(new AppUser()
			{

				Id = Guid.NewGuid().ToString(),
				NameSurname = model.NameSurname,

				UserName = model.UserName,
				Email = model.Email
			}, model.Password);

			var response = new CreateUserResponse() { Succeeded = result.Succeeded };

			if (result.Succeeded)
			{
				response.Message = "Successfully created";
			}
			else
			{
				foreach (var error in result.Errors)
				{
					response.Message += $"{error.Code} - {error.Code}\n";
				}
			}
			return response;

		}

		/// <summary>
		///  Updates the specified <paramref name="user"/> refresh token.
		/// </summary>
		/// <param name="refreshToken"> Refresh token.</param>
		/// <param name="user">The user to update.</param>
		/// <param name="accessTokenDate">Access token expire date.</param>
		/// <param name="addOnAccessToken"> Time in minutes to add on access token date</param>
		/// <returns>The <see cref="Task"/> that represents the asynchronous operation</returns>
		/// <exception cref="UserNotFoundException"></exception>
		public async Task UpdateRefreshToken(string refreshToken, AppUser? user, DateTime accessTokenDate, int addOnAccessToken)
		{
			if (user != null)
			{
				user.RefreshToken = refreshToken;
				user.RefreshTokenExpireDate = accessTokenDate.AddSeconds(addOnAccessToken);

				await _userManager.UpdateAsync(user);
			}
			else
			{
				throw new UserNotFoundException();
			}
		}

		public async Task AssignRolesToUser(string userId, List<RoleDTO>? roles)
		{
			var user = await _userManager.FindByIdAsync(userId) ?? throw new UserNotFoundException();

			var userRoles = await _userManager.GetRolesAsync(user);
			await _userManager.RemoveFromRolesAsync(user, userRoles);
			await _userManager.AddToRolesAsync(user, roles?.Select(r => r.Name)!);
		}

		public async Task<string[]> GetRolesToUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId) ?? throw new UserNotFoundException();

			var roles = await _userManager.GetRolesAsync(user);
			return [.. roles];

		}

		public async Task<bool> HasPermissionToEndpointAsync(string userNameOrId, string endpointCode)
		{
			var user =
				await _userManager.FindByNameAsync(userNameOrId)
				??
				await _userManager.FindByIdAsync(userNameOrId);

			if (user != null)
			{
				var userRoles = await GetRolesToUserAsync(user.Id);
				if (userRoles.Length == 0)
				{
					return false;
				}
				if (userRoles.Any(r => r == "Admin"))
				{
					return true;
				}

				var endpoint = await _endpointReadRepository.Table
					.Include(e => e.Roles)
					.FirstOrDefaultAsync(e => e.Code == endpointCode);
				if (endpoint == null)
				{
					return false;
				}

				var endpointRoles = endpoint.Roles.Select(r => r.Name);

				foreach (var userRole in userRoles)
				{
					foreach (var endpointRole in endpointRoles)
					{
						if (userRole == endpointRole)
						{
							return true;
						}
					}
				}
				return false;
			}
			else
			{
				return false;
			}
		}
	}
}
