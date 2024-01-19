using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace E_Commerce.API.Configurations.Serilog
{
	public class UserEmailColumnWriter : ColumnWriterBase
	{
		public UserEmailColumnWriter(NpgsqlDbType dbType, int? columnLength = null) : base(dbType, columnLength)
		{
		}

		public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
		{
			var (userEmail, value) = logEvent.Properties.FirstOrDefault(property => property.Key == "user_email");

			return value?.ToString() ?? null;

		}
	}
}
