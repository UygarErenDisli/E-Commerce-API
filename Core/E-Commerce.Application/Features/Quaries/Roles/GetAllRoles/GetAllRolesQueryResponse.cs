using E_Commerce.Application.DTOs.Role;

namespace E_Commerce.Application.Features.Quaries.Roles.GetAllRoles
{
	public class GetAllRolesQueryResponse
	{
        public int TotalCount { get; set; }
        public List<RoleDTO> Roles{ get; set; }
    }
}