namespace E_Commerce.Application.DTOs.User
{
	public class ListUsersDTO
	{
		public int TotalCount { get; set; }
		public List<ListUserDTO> Users { get; set; }
	}

	public class ListUserDTO
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string NameSurname { get; set; }
		public string Email { get; set; }
	}
}
