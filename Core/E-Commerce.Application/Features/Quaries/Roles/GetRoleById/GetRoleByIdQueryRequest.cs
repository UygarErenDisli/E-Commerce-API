using MediatR;

namespace E_Commerce.Application.Features.Quaries.Roles.GetRoleById
{
	public class GetRoleByIdQueryRequest : IRequest<GetRoleByIdQueryResponse>
	{
        public string Id { get; set; }
    }
}