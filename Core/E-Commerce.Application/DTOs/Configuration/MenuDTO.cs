namespace E_Commerce.Application.DTOs.Configuration
{
	public class MenuDTO
	{
		public string Name { get; set; }
		public List<Action> Actions { get; set; } = [];
	}
}
