using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Products.UpdateProduct
{
	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly IProductWriteRepository _productWriteRepository;

		public UpdateProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
		{
			_productReadRepository = productReadRepository;
			_productWriteRepository = productWriteRepository;
		}

		public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
		{
			Product product = await _productReadRepository.GetByIdAsync(request.Id);

			product.Name = request.Name;
			product.Stock = request.Stock;
			product.Price = request.Price;

			_productWriteRepository.Update(product);

			await _productWriteRepository.SaveAsync();

			return new();
		}
	}
}
