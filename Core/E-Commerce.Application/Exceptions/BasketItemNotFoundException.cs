namespace E_Commerce.Application.Exceptions
{
	public class BasketItemNotFoundException : Exception
	{
		public BasketItemNotFoundException() : base("Basket Item is not found")
		{
		}

		public BasketItemNotFoundException(string? message) : base(message)
		{
		}

		public BasketItemNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
