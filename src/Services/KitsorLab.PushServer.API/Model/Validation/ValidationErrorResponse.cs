namespace KitsorLab.PushServer.API.Model.Validation
{
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using System.Collections.Generic;
	using System.Linq;

	public class ValidationErrorResponse
	{
		public string Message { get; }
		public List<ValidationError> Errors { get; }

		public ValidationErrorResponse(ModelStateDictionary modelState)
		{
			Message = "Validation Failed";
			Errors = modelState.Keys
							.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
							.ToList();
		}
	}
}
