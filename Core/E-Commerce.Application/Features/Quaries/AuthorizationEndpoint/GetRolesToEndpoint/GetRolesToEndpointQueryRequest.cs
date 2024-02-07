using MediatR;

namespace E_Commerce.Application.Features.Quaries.AuthorizationEndpoint.GetRolesToEndpoint
{
	public class GetRolesToEndpointQueryRequest : IRequest<List<GetRolesToEndpointQueryResponse>>
	{
		public string EndpointCode { get; set; }
		public string MenuName { get; set; }
	}
}