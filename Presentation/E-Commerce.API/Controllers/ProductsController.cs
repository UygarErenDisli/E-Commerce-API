using E_Commerce.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly IProductWriteRepository _productWriteRepository;

		private readonly IOrderReadRepository _orderReadRepository;
		private readonly IOrderWriteRepository _orderWriteRepository;

		private readonly ICustomerReadRepository _customerReadRepository;
		private readonly ICustomerWriteRepository _customerWriteRepository;

		public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
			_orderReadRepository = orderReadRepository;
			_orderWriteRepository = orderWriteRepository;
			_customerReadRepository = customerReadRepository;
			_customerWriteRepository = customerWriteRepository;
		}

		[HttpGet]
		public async Task<ActionResult> Get()
		{
			//await _productWriteRepository.AddRangeAsync(new()
			//{
			//	new(){Id = Guid.NewGuid(),Name="Test Product 1",Price=10,Stock=10},
			//	new(){Id = Guid.NewGuid(),Name="Test Product 2",Price=12,Stock=10},
			//	new(){Id = Guid.NewGuid(),Name="Test Product 3",Price=13,Stock=10},
			//	new(){Id = Guid.NewGuid(),Name="Test Product 4",Price=14,Stock=10},
			//	new(){Id = Guid.NewGuid(),Name="Test Product 5",Price=15,Stock=10}
			//});
			//var products = _productReadRepository.GetAll(false);
			//await _customerWriteRepository.AddAsync(
			//	new() { Id = customerid, Name = "Uygar" });
			//var customer = _customerReadRepository.GetAll();

			//var customerid = "73ff67ae-9155-4355-8dca-746133d6c194";
			//var address = new Address() { City = "Istanbul", Country = "Turkey", State = "Turkey", Street = "Zafer", ZipCode = "10012" };
			//await _orderWriteRepository.AddAsync(new()
			//{
			//	Id = Guid.NewGuid(),
			//	Address = address,
			//	CustomerId = Guid.Parse(customerid),
			//	Description = "Test Order"
			//});
			//await _productWriteRepository.SaveAsync();

			var customer = _customerReadRepository.GetAll().First();
			customer.Name = "Uygar Updated";
			_customerWriteRepository.Update(customer);
			await _customerWriteRepository.SaveAsync();

			//var orders = _orderReadRepository.GetAll();

			return Ok();
		}
	}
}
