using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Products.CreateProduct
{
	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
	{
		private readonly IProductWriteRepository _productWriteRepository;

		public CreateProductCommandHandler(IProductWriteRepository productWriteRepository)
		{
			_productWriteRepository = productWriteRepository;
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

			return new();
		}
	}
}
