using E_Commerce.Application.Repositories;
using E_Commerce.Domain.Entities;
using MediatR;

namespace E_Commerce.Application.Features.Commands.Products.DeleteProduct
{
	public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
	{
		private readonly IProductReadRepository _productReadRepository;
		private readonly IProductWriteRepository _productWriteRepository;

		public DeleteProductCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
		{
			_productWriteRepository = productWriteRepository;
			_productReadRepository = productReadRepository;
		}

		public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
		{
			Product product = await _productReadRepository.GetByIdAsync(request.Id);

			_productWriteRepository.Remove(product);

			await _productWriteRepository.SaveAsync();

			return new();
		}
	}
}
