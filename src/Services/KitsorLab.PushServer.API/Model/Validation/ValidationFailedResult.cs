namespace KitsorLab.PushServer.API.Model.Validation
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ModelBinding;

	public class ValidationFailedResult : ObjectResult
	{
		public ValidationFailedResult(ModelStateDictionary modelState)
				: base(new ValidationErrorResponse(modelState))
		{
			StatusCode = StatusCodes.Status422UnprocessableEntity;
		}
	}
}
