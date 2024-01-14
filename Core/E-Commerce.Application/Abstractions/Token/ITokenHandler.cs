namespace E_Commerce.Application.Abstractions.Token
{
	public interface ITokenHandler
	{
		DTOs.Token CreateAccessToken(int minute);
	}
}
