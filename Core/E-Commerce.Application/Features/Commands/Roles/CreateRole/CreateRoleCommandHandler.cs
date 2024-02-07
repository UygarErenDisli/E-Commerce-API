using E_Commerce.Application.Abstractions.Services.Identity;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Roles.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
	{
		private readonly IRoleService _roleService;

		public CreateRoleCommandHandler(IRoleService roleService)
		{
			_roleService = roleService;
		}

		public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
		{
			var response = await _roleService.CreateRole(request.Name);
			return new()
			{
				Succeeded = response
			};
		}
	}
}
