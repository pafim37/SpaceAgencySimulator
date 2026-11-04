using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sas.Body.Core.Exceptions;

namespace Sas.Body.Service.Filters;

public sealed class BodyExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is OperationCanceledException && context.HttpContext.RequestAborted.IsCancellationRequested)
            return;

        string? action = context.RouteData.Values["action"]?.ToString();
        context.Result = context.Exception switch
        {
            BodyAlreadyExistsException exception => new ConflictObjectResult(new { message = exception.Message }),
            NoBodyInDatabaseException exception => new NotFoundObjectResult(exception.Message),
            ArgumentException exception when action == "Patch" =>
                new UnprocessableEntityObjectResult(new { message = exception.Message }),
            Exception exception when action == "Clone" => new BadRequestObjectResult(exception.Message),
            Exception exception => new ObjectResult(new { message = exception.Message }) { StatusCode = 500 }
        };
        context.ExceptionHandled = true;
    }
}
