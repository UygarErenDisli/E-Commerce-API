using MediatR;

namespace E_Commerce.Application.Features.Quaries.IdentityUser.GetUserNotifications
{
	public class GetUserNotificationsQueryRequest : IRequest<List<GetUserNotificationsQueryResponse>>
	{
	}
}