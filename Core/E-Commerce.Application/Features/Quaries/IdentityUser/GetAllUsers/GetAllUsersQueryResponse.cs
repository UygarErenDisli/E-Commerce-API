namespace E_Commerce.Application.Features.Quaries.IdentityUser.GetAllUsers
{
	public class GetAllUsersQueryResponse
	{
		public int TotalCount { get; set; }
		public List<GetAllUserQueryResponse> Users { get; set; }
	}
	public class GetAllUserQueryResponse
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string NameSurname { get; set; }
		public string Email { get; set; }
	}
}