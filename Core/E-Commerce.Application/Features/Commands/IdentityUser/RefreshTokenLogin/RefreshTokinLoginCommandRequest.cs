using MediatR;

namespace E_Commerce.Application.Features.Commands.IdentityUser.RefreshTokenLogin
{
	public class RefreshTokinLoginCommandRequest : IRequest<RefreshTokinLoginCommandResponse>
	{
		public string RefreshToken { get; set; }
	}
}
