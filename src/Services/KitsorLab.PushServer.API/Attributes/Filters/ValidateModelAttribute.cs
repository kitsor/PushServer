namespace KitsorLab.PushServer.API.Attributes.Filters
{
	using KitsorLab.PushServer.API.Model.Validation;
	using Microsoft.AspNetCore.Mvc.Filters;

	public class ValidateModelAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				context.Result = new ValidationFailedResult(context.ModelState);
			}
		}
	}
}
