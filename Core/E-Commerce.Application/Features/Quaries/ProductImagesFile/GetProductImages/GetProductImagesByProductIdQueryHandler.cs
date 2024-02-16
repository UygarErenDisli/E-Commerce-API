using E_Commerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Application.Features.Quaries.ProductImagesFile.GetProductImages
{
	public class GetProductImagesByProductIdQueryHandler : IRequestHandler<GetProductImagesByProductIdQueryRequest, List<GetProductImagesByProductIdQueryResponse>>
	{
		private readonly IProductWriteRepository _productWriteRepository;
		private readonly IConfiguration _configuration;

		public GetProductImagesByProductIdQueryHandler(IProductWriteRepository productWriteRepository, IConfiguration configuration)
		{
			_productWriteRepository = productWriteRepository;
			_configuration = configuration;
		}

		public async Task<List<GetProductImagesByProductIdQueryResponse>> Handle(GetProductImagesByProductIdQueryRequest request, CancellationToken cancellationToken)
		{
			var productFromDb = await _productWriteRepository.Table
				.Include(p => p.ProductImages)
				.FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

			List<GetProductImagesByProductIdQueryResponse>? output;
			if (!string.IsNullOrEmpty(_configuration["StorageBaseUrl"]))
			{
				output = productFromDb?.ProductImages.Select(i => new GetProductImagesByProductIdQueryResponse()
				{
					Id = i.Id,
					Path = $"{_configuration["StorageBaseUrl"]}/{i.Path}",
					FileName = i.FileName,
					IsShowCaseImage = i.IsShowCaseImage
				}).ToList();
			}
			else
			{
				output = productFromDb?.ProductImages.Select(i => new GetProductImagesByProductIdQueryResponse()
				{
					Id = i.Id,
					Path = i.Path,
					FileName = i.FileName,
					IsShowCaseImage = i.IsShowCaseImage
				}).ToList();
			}
			return output ?? [];
		}
	}
}
