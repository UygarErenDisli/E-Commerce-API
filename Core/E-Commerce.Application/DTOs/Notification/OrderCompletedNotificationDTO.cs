namespace E_Commerce.Application.DTOs.Notification
{
	public class OrderCompletedNotificationDTO
	{
		public string OrderCode { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public DateTime OrderDate { get; set; }
	}
}
