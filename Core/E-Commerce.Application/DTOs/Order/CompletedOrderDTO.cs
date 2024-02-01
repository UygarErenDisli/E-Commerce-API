namespace E_Commerce.Application.DTOs.Order
{
	public class CompletedOrderDTO
	{
		public string OrderCode { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public DateTime OrderDate { get; set; }
	}
}
