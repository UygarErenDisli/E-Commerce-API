using E_Commerce.Application.Abstractions.Services.Identity;
using MediatR;

namespace E_Commerce.Application.Features.Quaries.Roles.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQueryRequest, GetRoleByIdQueryResponse>
	{
		private readonly IRoleService _roleService;

		public GetRoleByIdQueryHandler(IRoleService roleService)
		{
			_roleService = roleService;
		}

		public async Task<GetRoleByIdQueryResponse> Handle(GetRoleByIdQueryRequest request, CancellationToken cancellationToken)
		{
			var response = await _roleService.GetRoleById(request.Id);
			return new()
			{
				Id = response.Id,
				Name = response.Name
			};
		}
	}
}
