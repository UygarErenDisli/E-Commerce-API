using E_Commerce.Application.Abstractions.Services.Identity;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.IdentityUser.GetRolesToUserAsync
{
	public class GetRolesToUserQueryHandler : IRequestHandler<GetRolesToUserQueryRequest, GetRolesToUserQueryResponse>
	{
		private readonly IUserService _userService;

		public GetRolesToUserQueryHandler(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<GetRolesToUserQueryResponse> Handle(GetRolesToUserQueryRequest request, CancellationToken cancellationToken)
		{
			var response = await _userService.GetRolesToUserAsync(request.UserId);
			return new()
			{
				Roles = response
			};

		}
	}
}
