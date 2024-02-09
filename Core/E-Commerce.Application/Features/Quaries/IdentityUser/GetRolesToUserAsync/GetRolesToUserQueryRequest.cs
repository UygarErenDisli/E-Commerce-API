using MediatR;

namespace E_Commerce.Application.Features.Quaries.IdentityUser.GetRolesToUserAsync
{
	public class GetRolesToUserQueryRequest : IRequest<GetRolesToUserQueryResponse>
	{
		public string UserId { get; set; }
	}
}