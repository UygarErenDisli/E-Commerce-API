namespace E_Commerce.Application.Features.Quaries.IdentityUser.GetUserNotifications
{
	public class GetUserNotificationsQueryResponse
	{
        public string Id { get; set; }
        public string Subject { get; set; }
		public string Message { get; set; }
		public string NotificationDate { get; set; }
	}
}