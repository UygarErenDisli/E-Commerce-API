using E_Commerce.Application.Abstractions.Services.Authorization;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.AuthorizationEndpoint.GetRolesToEndpoint
{
	public class GetRolesToEndpointQueryHandler : IRequestHandler<GetRolesToEndpointQueryRequest, List<GetRolesToEndpointQueryResponse>>
	{
		private readonly IAuthorizationEndpointService _authorizationEndpointService;

		public GetRolesToEndpointQueryHandler(IAuthorizationEndpointService authorizationEndpointService)
		{
			_authorizationEndpointService = authorizationEndpointService;
		}

		public async Task<List<GetRolesToEndpointQueryResponse>> Handle(GetRolesToEndpointQueryRequest request, CancellationToken cancellationToken)
		{
			var response = await _authorizationEndpointService.GetRolesToEndpointAsync(request.EndpointCode, request.MenuName);

			return response.Select(r => new GetRolesToEndpointQueryResponse()
			{
				Id = r.Id,
				Name = r.Name
			}).ToList();
		}
	}
}
