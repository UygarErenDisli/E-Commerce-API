namespace E_Commerce.Application.Exceptions
{
	public class RoleNotFoundException : Exception
	{
		public RoleNotFoundException() : base("Role not found!")
		{
		}

		public RoleNotFoundException(string? message) : base(message)
		{
		}

		public RoleNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
