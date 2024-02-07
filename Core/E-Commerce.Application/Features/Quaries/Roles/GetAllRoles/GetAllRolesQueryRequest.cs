using MediatR;

namespace E_Commerce.Application.Features.Quaries.Roles.GetAllRoles
{
	public class GetAllRolesQueryRequest : IRequest<GetAllRolesQueryResponse>
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
	}
}