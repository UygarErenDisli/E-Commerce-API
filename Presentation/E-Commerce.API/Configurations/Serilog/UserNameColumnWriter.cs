using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace E_Commerce.API.Configurations.Serilog
{
	public class UserNameColumnWriter : ColumnWriterBase
	{
		public UserNameColumnWriter(NpgsqlDbType dbType, int? columnLength = null) : base(dbType, columnLength)
		{
		}

		public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
		{
			var (username, value) = logEvent.Properties.FirstOrDefault(property => property.Key == "user_name");

			return value?.ToString() ?? null;
		}
	}
}
