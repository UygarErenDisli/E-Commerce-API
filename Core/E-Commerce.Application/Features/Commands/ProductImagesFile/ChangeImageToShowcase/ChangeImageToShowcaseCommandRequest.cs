using MediatR;

namespace E_Commerce.Application.Features.Commands.ProductImagesFile.ChangeImageToShowcase
{
	public class ChangeImageToShowcaseCommandRequest : IRequest<ChangeImageToShowcaseCommandResponse>
	{
        public string ImageId { get; set; }
        public string ProductId { get; set; }
    }
}
