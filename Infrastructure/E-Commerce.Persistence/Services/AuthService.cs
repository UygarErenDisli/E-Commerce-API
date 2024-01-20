using E_Commerce.Application.Abstractions.Services;
using E_Commerce.Application.Abstractions.Token;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Exceptions;
using E_Commerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Persistence.Services
{

	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenHandler _tokenHandler;
		private readonly IConfiguration _configuration;
		private readonly IUserService _userService;

		public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, IConfiguration configuration, IUserService userService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenHandler = tokenHandler;
			_configuration = configuration;
			_userService = userService;
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
				var token = _tokenHandler.CreateAccessToken(accessTokenLifetimeInMinutes, user);
				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);
				return token;
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

			var token = _tokenHandler.CreateAccessToken(accessTokenLifeTimeInMinutes, appUser);
			await _userService.UpdateRefreshToken(token.RefreshToken, appUser, token.Expiration, 300);
			return token;

		}

		public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
		{

			var user = await _userManager.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
			if (user == null)
			{
				throw new UserNotFoundException();
			}
			else if (user.RefreshTokenExpireDate > DateTime.UtcNow)
			{
				var token = _tokenHandler.CreateAccessToken(15, user);
				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);
				return token;
			}
			else
			{
				throw new RefreshTokenExpiredException();
			}
		}
	}
}
