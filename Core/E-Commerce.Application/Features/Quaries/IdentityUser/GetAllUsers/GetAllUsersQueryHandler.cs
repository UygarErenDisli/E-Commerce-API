using E_Commerce.Application.Abstractions.Services.Identity;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.IdentityUser.GetAllUsers
{
	public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
	{
		private readonly IUserService _userService;

		public GetAllUsersQueryHandler(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
		{
			var response = await _userService.GetAllUsers(request.PageIndex, request.PageSize);

			return new()
			{
				TotalCount = response.TotalCount,
				Users = response.Users.Select(r => new GetAllUserQueryResponse()
				{
					Email = r.Email,
					NameSurname = r.NameSurname,
					UserId = r.UserId,
					UserName = r.UserName
				}).ToList()
			};
		}
	}
}
