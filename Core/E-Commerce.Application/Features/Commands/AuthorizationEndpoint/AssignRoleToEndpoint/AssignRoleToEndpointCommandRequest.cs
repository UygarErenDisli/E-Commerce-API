using E_Commerce.Application.DTOs.Role;
using MediatR;

namespace E_Commerce.Application.Features.Commands.AuthorizationEndpoint.AssignRoleToEndpoint
{
	public class AssignRoleToEndpointCommandRequest : IRequest<AssignRoleToEndpointCommandResponse>
	{
        public string MenuName { get; set; }
        public string EndpointCode { get; set; }
        public List<RoleDTO>? Roles{ get; set; }
        public Type? Type { get; set; }
    }
}