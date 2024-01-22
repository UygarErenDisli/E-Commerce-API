namespace E_Commerce.Application.Exceptions
{
	public class ProductImageNotFoundException : Exception
	{
		public ProductImageNotFoundException() : base("Product image not found")
		{
		}

		public ProductImageNotFoundException(string? message) : base(message)
		{
		}

		public ProductImageNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
