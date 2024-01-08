using E_Commerce.Application.Validators.Products;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.Services.Storage.Azure;
using E_Commerce.Infrastructure.Validators;
using E_Commerce.Persistence.Extentions;
using FluentValidation.AspNetCore;

var MyAllowOrigins = "FrontendOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("PostgreSQL")!);

builder.Services.AddInfrastractureServices();
//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();


builder.Services.AddCors(options =>
{
	options.AddPolicy(MyAllowOrigins, policy =>
	{
		policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod();
	});
});

builder.Services
	.AddControllers(configuration => configuration.Filters.Add<ValidationFilter>())
	.AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
	.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors(MyAllowOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
