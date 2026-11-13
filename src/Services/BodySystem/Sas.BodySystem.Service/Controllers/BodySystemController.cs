using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.BodySystem.Process.Models.Queries;

namespace Sas.BodySystem.Process.Controllers;

[ApiController]
[Route("body-system")]
public class BodySystemController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromBody] double gravitationalConstant, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new CreateBodySystem(gravitationalConstant), cancellationToken));
}
