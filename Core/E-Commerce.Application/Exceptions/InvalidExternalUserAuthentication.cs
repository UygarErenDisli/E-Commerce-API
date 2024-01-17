namespace E_Commerce.Application.Exceptions
{
	public class InvalidExternalUserAuthentication : Exception
	{
		public InvalidExternalUserAuthentication() : base("Invalid External User Authentication")
		{
		}

		public InvalidExternalUserAuthentication(string? message) : base(message)
		{
		}

		public InvalidExternalUserAuthentication(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
