namespace E_Commerce.Application.Features.Commands.IdentityUser.GoogleLogin
{
	public class GoogleLoginCommandResponse
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime Expiration { get; set; }
	}
}
