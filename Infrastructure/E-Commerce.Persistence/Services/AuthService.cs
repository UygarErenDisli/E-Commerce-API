using E_Commerce.Application.Abstractions.Services;
using E_Commerce.Application.Abstractions.Token;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Exceptions;
using E_Commerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Persistence.Services
{

	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenHandler _tokenHandler;
		private readonly IConfiguration _configuration;

		public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenHandler = tokenHandler;
			_configuration = configuration;
		}

		public async Task<Token> LoginUser(string userNameOrEmail, string password, int accessTokenLifetimeInMinutes)
		{
			var user = await _userManager.FindByNameAsync(userNameOrEmail)
				?? await _userManager.FindByEmailAsync(userNameOrEmail);
			if (user == null)
			{
				throw new UserNotFoundException();
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

			if (result.Succeeded)
			{
				return _tokenHandler.CreateAccessToken(accessTokenLifetimeInMinutes);
			}
			throw new UserNotFoundException();
		}
		public async Task<Token> GoogleLogin(string idToken, int accessTokenLifeTimeInMinutes)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings()
			{
				Audience = new List<string> { _configuration["ExternalLogin:GoogleClientId"]! }
			};


			var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

			var userLoginInfo = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

			var appUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

			bool result = appUser != null;

			if (appUser == null)
			{
				appUser = await _userManager.FindByEmailAsync(payload.Email);


				if (appUser == null)
				{

					appUser = new AppUser()
					{
						Id = Guid.NewGuid().ToString(),
						Email = payload.Email,
						UserName = payload.Email,
						NameSurname = payload.Name,
					};

					var userCreation = await _userManager.CreateAsync(appUser);
					result = userCreation.Succeeded;
				}
			}

			if (result)
			{
				await _userManager.AddLoginAsync(appUser, userLoginInfo);
			}
			else
			{
				throw new InvalidExternalUserAuthentication();
			}

			var token = _tokenHandler.CreateAccessToken(accessTokenLifeTimeInMinutes);

			return token;

		}

	}
}
