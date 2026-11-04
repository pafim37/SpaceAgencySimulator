using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.Body.Business.Models.Queries;

namespace Sas.Body.Service.Controllers;

[ApiController]
[Route("body-system")]
public class BodySystemController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetBodySystem(), cancellationToken));

    [HttpGet("{atTime}")]
    public async Task<IActionResult> GetAtTime(double atTime, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetBodySystem(atTime), cancellationToken));
}
