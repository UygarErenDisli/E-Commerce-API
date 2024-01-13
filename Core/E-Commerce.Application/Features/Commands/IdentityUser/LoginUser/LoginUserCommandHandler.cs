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

		public LoginUserCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
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
			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

			if (result.Succeeded)
			{
				///
				throw new NotImplementedException();
			}
			return new();
		}
	}
}
