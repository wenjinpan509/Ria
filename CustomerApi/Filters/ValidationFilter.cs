
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerApi.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .SelectMany(kvp => kvp.Value?.Errors.Select(e => $"{kvp.Key}: {e.ErrorMessage}") ?? Enumerable.Empty<string>())
                .ToList();

            context.Result = new BadRequestObjectResult(new { errors });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) 
    { 
    }
}

