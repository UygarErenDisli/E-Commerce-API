using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.UploadProductImages
{
	public class UploadProductImagesCommandRequest : IRequest<UploadProductImagesCommandResponse>
	{
		public string Id { get; set; }
		public IFormFileCollection? Files { get; set; }
	}
}
