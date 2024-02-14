using E_Commerce.Application.Features.Commands.Products.CreateProduct;
using FluentValidation;

namespace E_Commerce.Application.Validators.Products
{
	public class CreateProductCommandRequestValidator : AbstractValidator<CreateProductCommandRequest>
	{
		public CreateProductCommandRequestValidator()
		{
			RuleFor(p => p.Name)
				.NotEmpty().NotNull().WithMessage("Plese enter a Name for your Product")
				.MinimumLength(5).MaximumLength(150).WithMessage("Product name should be in range of 5 characters to 150 ");

			RuleFor(p => p.Stock)
				.NotEmpty().NotNull().WithMessage("Please enter a Stock value for your Product")
				.Must(stock => stock >= 0).WithMessage("Please enter a valid Stock value for your Product");

			RuleFor(p => p.Price)
				.NotEmpty().NotNull().WithMessage("Please enter a Price value for your Product")
				.Must(price => price >= 0).WithMessage("Please enter a valid Price value for your Product");

		}
	}
}
