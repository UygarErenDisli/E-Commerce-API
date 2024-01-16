namespace E_Commerce.Application.Exceptions
{
	public class InvalidExternalAuthentication : Exception
	{
		public InvalidExternalAuthentication() : base("Invalid External Authentication")
		{
		}

		public InvalidExternalAuthentication(string? message) : base(message)
		{
		}

		public InvalidExternalAuthentication(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
