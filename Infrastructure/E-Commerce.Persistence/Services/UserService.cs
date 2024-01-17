using E_Commerce.Application.Abstractions.Services;
using E_Commerce.Application.DTOs.User;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Persistence.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;

		public UserService(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
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
	}
}
