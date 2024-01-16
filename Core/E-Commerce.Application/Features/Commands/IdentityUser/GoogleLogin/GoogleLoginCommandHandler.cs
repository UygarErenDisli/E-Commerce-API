using E_Commerce.Application.Abstractions.Token;
using E_Commerce.Application.Exceptions;
using E_Commerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Application.Features.Commands.IdentityUser.GoogleLogin
{
	public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenHandler _tokenHandler;
		private readonly IConfiguration _configuration;

		public GoogleLoginCommandHandler(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration)
		{
			_userManager = userManager;
			_tokenHandler = tokenHandler;
			_configuration = configuration;
		}

		public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings()
			{
				Audience = new List<string> { _configuration["ExternalLogin:GoogleClientId"]! }
			};


			var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);


			var userInfo = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

			var user = await _userManager.FindByLoginAsync(userInfo.LoginProvider, userInfo.ProviderKey);

			bool result = user != null;

			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(payload.Email);


				if (user == null)
				{

					user = new AppUser()
					{
						Id = Guid.NewGuid().ToString(),
						Email = payload.Email,
						UserName = payload.Email,
						NameSurname = payload.Name,
					};


					var userCreation = await _userManager.CreateAsync(user);
					result = userCreation.Succeeded;

				}
			}

			if (result)
			{
				await _userManager.AddLoginAsync(user, userInfo);

			}
			else
			{
				throw new InvalidExternalAuthentication();
			}
			var token = _tokenHandler.CreateAccessToken(5);
			return new()
			{
				AccessToken = token.AccessToken
			};
		}
	}
}
