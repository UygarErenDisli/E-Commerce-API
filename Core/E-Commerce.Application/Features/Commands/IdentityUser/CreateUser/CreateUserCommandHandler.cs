using E_Commerce.Application.Abstractions.Services.Identity;
using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
	{
		private readonly IUserService _userService;

		public CreateUserCommandHandler(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
		{
			var createUserResponse = await _userService.CreateUser(new()
			{
				Email = request.Email,
				NameSurname = request.NameSurname,
				Password = request.Password,
				UserName = request.UserName
			});


			return new()
			{
				Message = createUserResponse.Message,
				Succeeded = createUserResponse.Succeeded
			};
		}
	}
}
