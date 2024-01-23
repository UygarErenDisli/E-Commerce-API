namespace E_Commerce.Application.Exceptions
{
	public class BasketNotFoundException : Exception
	{
		public BasketNotFoundException() : base("Basket is not found")
		{
		}

		public BasketNotFoundException(string? message) : base(message)
		{
		}

		public BasketNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
