using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Commerce.Infrastructure.Validators
{
	public class ValidationFilter : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (!context.ModelState.IsValid)
			{
				var errors = context.ModelState
					.Where(e => e.Value.Errors.Any())
					.ToDictionary(e => e.Key, e => e.Value.Errors.Select(e => e.ErrorMessage))
					.ToArray();

				context.Result = new BadRequestObjectResult(errors);
				return;
			};
			await next();
		}
	}
}
