namespace E_Commerce.Application.Abstractions.Hubs
{
	public interface IProductHubService
	{
		Task ProductAddedMessageAsync(string message);
	}
}
