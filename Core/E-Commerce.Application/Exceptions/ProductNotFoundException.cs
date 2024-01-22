namespace E_Commerce.Application.Exceptions
{
	public class ProductNotFoundException : Exception
	{
		public ProductNotFoundException() : base("Couln't found product")
		{
		}

		public ProductNotFoundException(string? message) : base(message)
		{
		}

		public ProductNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
