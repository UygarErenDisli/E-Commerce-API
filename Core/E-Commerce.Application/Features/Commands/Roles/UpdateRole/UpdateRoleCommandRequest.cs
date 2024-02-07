using MediatR;

namespace E_Commerce.Application.Features.Commands.Roles.UpdateRole
{
	public class UpdateRoleCommandRequest : IRequest<UpdateRoleCommandResponse>
	{
        public string Id { get; set; }
        public string NewName { get; set; }
    }
}