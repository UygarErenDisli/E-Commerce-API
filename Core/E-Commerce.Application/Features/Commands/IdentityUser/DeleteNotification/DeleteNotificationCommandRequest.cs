using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.DeleteNotification
{
	public class DeleteNotificationCommandRequest : IRequest<DeleteNotificationCommandResponse>
	{
        public string Id { get; set; }
    }
}