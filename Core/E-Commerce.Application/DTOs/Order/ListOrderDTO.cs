namespace E_Commerce.Application.DTOs.Order
{
	public class ListOrderDTO
	{
		public string Id { get; set; }
		public string OrderCode { get; set; }
		public string Username { get; set; }
		public string UserEmail { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
