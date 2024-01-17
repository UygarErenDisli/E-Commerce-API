using E_Commerce.Application.Abstractions.Services.Authentication;

namespace E_Commerce.Application.Abstractions.Services
{
	public interface IAuthService : IExternalAuthentication, IInternalAuthentication
	{ }
}
