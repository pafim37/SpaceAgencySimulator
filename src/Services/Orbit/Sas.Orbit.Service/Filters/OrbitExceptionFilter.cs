using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sas.Orbit.Service.Controllers;
using Sas.Orbit.Service.Exceptions;

namespace Sas.Orbit.Service.Filters;

public sealed class OrbitExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is OperationCanceledException && context.HttpContext.RequestAborted.IsCancellationRequested)
        {
            return;
        }

        string? action = context.RouteData.Values["action"]?.ToString();
        context.Result = context.Exception switch
        {
            UnknownOrbitTypeException =>
                new BadRequestObjectResult("Cannot predict orbit type. Unsupported value of eccentricity."),
            InvalidOperationException when action == nameof(OrbitController.GetTwoBodyProblemOrbit) =>
                new BadRequestObjectResult("Cannot predict center of the orbit"),
            Exception exception => new ObjectResult(new { message = exception.Message }) { StatusCode = 500 }
        };
        context.ExceptionHandled = true;
    }
}
