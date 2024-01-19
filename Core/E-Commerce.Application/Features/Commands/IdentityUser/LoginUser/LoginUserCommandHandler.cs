using E_Commerce.Application.Abstractions.Services.Authentication;
using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.LoginUser
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
	{
		private readonly IInternalAuthentication _internalAuthentication;

		public LoginUserCommandHandler(IInternalAuthentication internalAuthentication)
		{
			_internalAuthentication = internalAuthentication;
		}

		public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
		{
			var response = await _internalAuthentication.LoginUser(request.UserNameOrEmail, request.Password, 300);

			return new()
			{
				AccessToken = response.AccessToken,
				RefreshToken = response.RefreshToken,
				Expiration = response.Expiration
			};
		}
	}
}
