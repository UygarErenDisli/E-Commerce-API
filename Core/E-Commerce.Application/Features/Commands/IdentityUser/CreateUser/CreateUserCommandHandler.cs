using E_Commerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Application.Features.Commands.IdentityUser.CreateUser
{
	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
	{
		private readonly UserManager<AppUser> _userManager;

		public CreateUserCommandHandler(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
		{
			var result = await _userManager.CreateAsync(new AppUser()
			{

				Id = Guid.NewGuid().ToString(),
				NameSurname = request.NameSurname,

				UserName = request.UserName,
				Email = request.Email
			}, request.Password);

			var response = new CreateUserCommandResponse() { Succeeded = result.Succeeded };

			if (result.Succeeded)
			{
				response.Message = "Successfully created";
			}
			else
			{
				foreach (var error in result.Errors)
				{
					response.Message += $"{error.Code} - {error.Code}\n";
				}
			}
			return response;
		}
	}
}
