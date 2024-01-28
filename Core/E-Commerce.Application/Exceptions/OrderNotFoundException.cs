namespace E_Commerce.Application.Exceptions
{
	public class OrderNotFoundException : Exception
	{
		public OrderNotFoundException() : base("Order Not Found!")
		{
		}

		public OrderNotFoundException(string? message) : base(message)
		{
		}

		public OrderNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
