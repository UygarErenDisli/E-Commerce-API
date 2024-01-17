namespace E_Commerce.Application.Abstractions.Services.Authentication
{
	public interface IExternalAuthentication
	{
		Task<DTOs.Token> GoogleLogin(string idToken, int accessTokenLifeTimeInMinutes);
	}
}
