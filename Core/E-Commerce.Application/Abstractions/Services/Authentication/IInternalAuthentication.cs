namespace E_Commerce.Application.Abstractions.Services.Authentication
{
	public interface IInternalAuthentication
	{
		Task<DTOs.Token> LoginUser(string userNameOrEmail, string password, int accessTokenLifetimeInMinutes);
	}
}
