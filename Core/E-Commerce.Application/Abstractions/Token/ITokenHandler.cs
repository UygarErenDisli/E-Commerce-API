using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Application.Abstractions.Token
{
	public interface ITokenHandler
	{
		DTOs.Token CreateAccessToken(int minute, AppUser user);
		string CreateRefreshToken();
	}
}
