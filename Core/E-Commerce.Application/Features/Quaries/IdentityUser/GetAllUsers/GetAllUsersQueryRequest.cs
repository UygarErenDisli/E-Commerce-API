using MediatR;

namespace E_Commerce.Application.Features.Quaries.IdentityUser.GetAllUsers
{
	public class GetAllUsersQueryRequest : IRequest<GetAllUsersQueryResponse>
	{
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}