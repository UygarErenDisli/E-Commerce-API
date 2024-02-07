using E_Commerce.Application.Enums;

namespace E_Commerce.Application.Attributes
{
	public class AuthorizeDefinitionAttribute : Attribute
	{
		public string Menu { get; set; }
		public string Definition { get; set; }
		public ActionType ActionType { get; set; }
	}
}
