using E_Commerce.API.Configurations.Serilog;
using E_Commerce.API.Extentions;
using E_Commerce.Application.Extentions;
using E_Commerce.Application.Validators.Products;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.Services.Storage.Azure;
using E_Commerce.Infrastructure.Validators;
using E_Commerce.Persistence.Extentions;
using E_Commerce.SignaR.Extentions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;


var MyAllowOrigins = "FrontendOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("PostgreSQL")!);

builder.Services.AddApplicationServices();

builder.Services.AddInfrastractureServices();
builder.Services.AddSignalRServices();

//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();


builder.Services.AddCors(options =>
{
	options.AddPolicy(MyAllowOrigins, policy =>
	{
		policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowCredentials().AllowAnyMethod();
	});
});

builder.Services
	.AddControllers(configuration => configuration.Filters.Add<ValidationFilter>())
	.AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
	.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);


Logger logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL")!, "Logs", needAutoCreateTable: true,
		columnOptions: new Dictionary<string, ColumnWriterBase>
		{
			{"message",new RenderedMessageColumnWriter(NpgsqlDbType.Text)},
			{"message_template",new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
			{"level",new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
			{"raise_date",new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
			{"exception",new ExceptionColumnWriter(NpgsqlDbType.Text) },
			{"log_event",new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
			{"user_name",new UserNameColumnWriter(NpgsqlDbType.Varchar) },
			{"user_email",new UserEmailColumnWriter(NpgsqlDbType.Varchar) }
		})
	.Enrich.FromLogContext()
	.MinimumLevel.Information()
	.CreateLogger();


builder.Host.UseSerilog(logger);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer("Admin", options =>
	{
		options.TokenValidationParameters = new()
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,

			ValidAudience = builder.Configuration["Token:Audience"],
			ValidIssuer = builder.Configuration["Token:Issuer"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SigninKey"]!)),
			LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,

			NameClaimType = ClaimTypes.Name,
		};
	});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}



app.UseSerilogRequestLogging();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(MyAllowOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) =>
{
	var userName = context.User?.Identity?.IsAuthenticated != null || true ? context.User!.Identity!.Name : null;

	var userEmail = context.User?.Claims != null || true ? context.User!.Claims!.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value : null;

	LogContext.PushProperty("user_name", userName);
	LogContext.PushProperty("user_email", userEmail);

	await next();
});


app.MapControllers();

app.MapHubs();

app.Run();
