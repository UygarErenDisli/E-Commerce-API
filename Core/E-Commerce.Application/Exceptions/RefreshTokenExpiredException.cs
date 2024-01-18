namespace E_Commerce.Application.Exceptions
{
	public class RefreshTokenExpiredException : Exception
	{
		public RefreshTokenExpiredException() : base("Refresh Token Is Expired")
		{
		}

		public RefreshTokenExpiredException(string? message) : base(message)
		{
		}

		public RefreshTokenExpiredException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

	}
}
