namespace E_Commerce.Application.Exceptions
{
	public class ActionNotFoundException : Exception
	{
		public ActionNotFoundException() : base("Action not found!")
		{
		}

		public ActionNotFoundException(string? message) : base(message)
		{
		}

		public ActionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
