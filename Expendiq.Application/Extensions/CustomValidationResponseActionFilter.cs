using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Expendiq.Application.Extensions
{
    public class CustomValidationResponseActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new List<string>();

                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                var responseObj = new
                {
                    Success = false,
                    Status = StatusCodes.Status400BadRequest,
                    ErrorMessage = string.Join(Environment.NewLine, errors),
                };

                context.Result = new BadRequestObjectResult(responseObj)
                {
                    StatusCode = 400
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}
