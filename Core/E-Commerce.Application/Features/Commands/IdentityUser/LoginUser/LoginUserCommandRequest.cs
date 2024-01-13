using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.LoginUser
{
	public class LoginUserCommandRequest : IRequest<LoginUserCommandResponse>
	{
		public string UserNameOrEmail { get; set; }
		public string Password { get; set; }
	}
}
