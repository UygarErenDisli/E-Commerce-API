using E_Commerce.Application.Abstractions.Services.Authentication;
using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.GoogleLogin
{
	public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
	{
		private readonly IExternalAuthentication _externalAuthentication;

		public GoogleLoginCommandHandler(IExternalAuthentication externalAuthentication)
		{
			_externalAuthentication = externalAuthentication;
		}

		public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
		{

			var response = await _externalAuthentication.GoogleLogin(request.IdToken, 1500);

			return new()
			{
				AccessToken = response.AccessToken,
				RefreshToken = response.RefreshToken,
				Expiration = response.Expiration
			};

		}
	}
}
