namespace E_Commerce.Application.Features.Commands.IdentityUser.RefreshTokenLogin
{
	public class RefreshTokinLoginCommandResponse
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime Expiration { get; set; }
	}
}
