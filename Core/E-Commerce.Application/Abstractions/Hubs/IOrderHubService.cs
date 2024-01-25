namespace E_Commerce.Application.Abstractions.Hubs
{
	public interface IOrderHubService
	{
		Task OrderCreatedMessageAsync(string message);
	}
}
