using E_Commerce.Application.DTOs.User;

namespace E_Commerce.Application.Abstractions.Services
{
	public interface IUserService
	{
		Task<CreateUserResponse> CreateUser(CreateUser model);
	}
}
