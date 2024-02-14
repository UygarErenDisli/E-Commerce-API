namespace E_Commerce.Application.Features.Commands.IdentityUser.LoginUser
{
	public class LoginUserCommandResponse
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime Expiration { get; set; }
		public bool HasAccessToAdminDashboard { get; set; }
	}
}
