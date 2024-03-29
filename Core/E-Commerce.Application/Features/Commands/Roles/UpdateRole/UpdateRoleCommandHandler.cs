﻿using E_Commerce.Application.Abstractions.Services.Identity;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Roles.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
	{
		private readonly IRoleService _roleService;

		public UpdateRoleCommandHandler(IRoleService roleService)
		{
			_roleService = roleService;
		}

		public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
		{
			var result = await _roleService.UpdateRole(request.Id, request.NewName);
			return new()
			{
				Succeeded = result
			};

		}
	}
}
