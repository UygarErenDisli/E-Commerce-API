using E_Commerce.Application.Abstractions.Services.Identity;
using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.AssignRolesToUser
{
	public class AssignRolesToUserCommandHandler : IRequestHandler<AssignRolesToUserCommandRequest, AssignRolesToUserCommandResponse>
	{
		private readonly IUserService _userService;

		public AssignRolesToUserCommandHandler(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<AssignRolesToUserCommandResponse> Handle(AssignRolesToUserCommandRequest request, CancellationToken cancellationToken)
		{
			await _userService.AssignRolesToUser(request.UserId, request.Roles);
			return new();
		}
	}
}
