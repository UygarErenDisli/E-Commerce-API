using E_Commerce.Application.Abstractions.Services;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.Roles.GetAllRoles
{
	public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQueryRequest, GetAllRolesQueryResponse>
	{
		private readonly IRoleService _roleService;

		public GetAllRolesQueryHandler(IRoleService roleService)
		{
			_roleService = roleService;
		}

		public async Task<GetAllRolesQueryResponse> Handle(GetAllRolesQueryRequest request, CancellationToken cancellationToken)
		{
			var response = await _roleService.GetAllRolesAsync(request.PageIndex, request.PageSize);
			return new()
			{
				Roles = response.Roles,
				TotalCount = response.TotalRoleCount
			};
		}
	}
}
