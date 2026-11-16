using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sas.Body.Core.Exceptions;

namespace Sas.BodySystem.Process.Filters;

public sealed class BodySystemExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is OperationCanceledException && context.HttpContext.RequestAborted.IsCancellationRequested)
        {
            return;
        }

        context.Result = context.Exception switch
        {
            BodyAlreadyExistsException exception => new ConflictObjectResult(new { message = exception.Message }),
            NoBodyInDatabaseException exception => new NotFoundObjectResult(exception.Message),
            Exception exception => new ObjectResult(new { message = exception.Message }) { StatusCode = 500 }
        };
        context.ExceptionHandled = true;
    }
}
