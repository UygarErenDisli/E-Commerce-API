using E_Commerce.Application.DTOs.Role;
using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.AssignRolesToUser
{
	public class AssignRolesToUserCommandRequest : IRequest<AssignRolesToUserCommandResponse>
	{
		public string UserId { get; set; }
		public List<RoleDTO>? Roles { get; set; }
	}
}
