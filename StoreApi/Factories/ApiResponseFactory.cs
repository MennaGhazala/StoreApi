using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using System.Net;

namespace StoreApi.Factories
{
    public class ApiResponseFactory
    {
        public static ActionResult CustomValidationErrorResponse(ActionContext Context)
        {

            var errors = Context.ModelState.Where(error => error.Value.Errors.Any())
                     .Select(error => new ValidationError
                     {
                         Key = error.Key,
                         Errors= error.Value.Errors.Select(e=>e.ErrorMessage)
                     });

            var validationResponse = new ValidationErrorResponse
            {
                StatuCode = (int)HttpStatusCode.BadRequest,
                Errors = errors,
                ErrorMessage=" Validation Failed"

            };

            return new BadRequestObjectResult(validationResponse);
        
        }
    }
}
