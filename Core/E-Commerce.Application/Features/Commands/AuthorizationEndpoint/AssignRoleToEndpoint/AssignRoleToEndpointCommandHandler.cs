using E_Commerce.Application.Abstractions.Services.Authorization;
using MediatR;

namespace E_Commerce.Application.Features.Commands.AuthorizationEndpoint.AssignRoleToEndpoint
{
	public class AssignRoleToEndpointCommandHandler : IRequestHandler<AssignRoleToEndpointCommandRequest, AssignRoleToEndpointCommandResponse>
	{
		private readonly IAuthorizationEndpointService _authorizationEndpointService;

		public AssignRoleToEndpointCommandHandler(IAuthorizationEndpointService authorizationEndpointService)
		{
			_authorizationEndpointService = authorizationEndpointService;
		}

		public async Task<AssignRoleToEndpointCommandResponse> Handle(AssignRoleToEndpointCommandRequest request, CancellationToken cancellationToken)
		{
			await _authorizationEndpointService.AssignRoleToEndpoint(request.Roles, request.MenuName, request.EndpointCode, request.Type!);
			return new();

		}
	}
}
