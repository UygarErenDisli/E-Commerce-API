namespace E_Commerce.Application.Exceptions
{
	public class InvalidUserIdException : Exception
	{
		public InvalidUserIdException() : base("Invalid User Id!!")
		{
		}

		public InvalidUserIdException(string? message) : base(message)
		{
		}

		public InvalidUserIdException(string? message, Exception? innerException) : base(message, innerException)
		{
		}


	}
}
