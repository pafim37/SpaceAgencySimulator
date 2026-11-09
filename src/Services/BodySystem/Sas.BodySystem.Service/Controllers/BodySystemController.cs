using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.BodySystem.Service.Models.Queries;

namespace Sas.BodySystem.Service.Controllers;

[ApiController]
[Route("body-system")]
public class BodySystemController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetBodySystem(AtTime: 0), cancellationToken));

    [HttpGet("{atTime}")]
    public async Task<IActionResult> GetAtTime(double atTime, CancellationToken cancellationToken)
    {
        if (!double.IsFinite(atTime) || atTime < 0)
        {
            return BadRequest("Time must be finite and non-negative.");
        }
        else
        {
            return Ok(await mediator.Send(new GetBodySystem(atTime), cancellationToken));
        }
    }
}
