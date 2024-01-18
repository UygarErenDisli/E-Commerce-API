using E_Commerce.Application.DTOs.User;
using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Application.Abstractions.Services
{
	public interface IUserService
	{
		Task<CreateUserResponse> CreateUser(CreateUser model);
		Task UpdateRefreshToken(string refreshToken, AppUser? user, DateTime accessTokenDate, int addOnAccessToken);
	}
}
