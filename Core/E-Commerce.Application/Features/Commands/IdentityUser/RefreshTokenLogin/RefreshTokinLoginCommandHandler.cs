using E_Commerce.Application.Abstractions.Services;
using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.RefreshTokenLogin
{
	public class RefreshTokinLoginCommandHandler : IRequestHandler<RefreshTokinLoginCommandRequest, RefreshTokinLoginCommandResponse>
	{
		private readonly IAuthService _authService;

		public RefreshTokinLoginCommandHandler(IAuthService authService)
		{
			_authService = authService;
		}

		public async Task<RefreshTokinLoginCommandResponse> Handle(RefreshTokinLoginCommandRequest request, CancellationToken cancellationToken)
		{
			var token = await _authService.RefreshTokenLoginAsync(request.RefreshToken);

			return new()
			{
				AccessToken = token.AccessToken,
				RefreshToken = token.RefreshToken,
				Expiration = token.Expiration
			};
		}
	}
}
