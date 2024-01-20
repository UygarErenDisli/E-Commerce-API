using E_Commerce.Application.Abstractions.Hubs;
using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Products.CreateProduct
{
	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
	{
		private readonly IProductWriteRepository _productWriteRepository;
		private readonly IProductHubService _productHubService;

		public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductHubService productHubService)
		{
			_productWriteRepository = productWriteRepository;
			_productHubService = productHubService;
		}

		public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
		{
			Product product = new()
			{
				Name = request.Name,
				Stock = request.Stock,
				Price = request.Price
			};
			await _productWriteRepository.AddAsync(product);

			await _productWriteRepository.SaveAsync();
			await _productHubService.ProductAddedMessageAsync($"Product added: {product.Name}");
			return new();
		}
	}
}
