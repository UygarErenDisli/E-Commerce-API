using E_Commerce.Application.Abstractions.Token;
using E_Commerce.Application.Exceptions;
using E_Commerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Application.Features.Commands.IdentityUser.LoginUser
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenHandler _tokenHandler;

		public LoginUserCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenHandler = tokenHandler;
		}

		public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByNameAsync(request.UserNameOrEmail);

			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
			}
			if (user == null)
			{
				throw new UserNotFoundException();
			}

			var token = _tokenHandler.CreateAccessToken(5);

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

			if (result.Succeeded)
			{
				return new()
				{
					AccessToken = token.AccessToken,
					Expiration = token.Expiration
				};
			}
			throw new UserNotFoundException();
		}
	}
}
