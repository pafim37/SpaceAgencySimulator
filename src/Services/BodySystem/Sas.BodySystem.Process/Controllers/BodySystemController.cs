using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.BodySystem.Process.Filters;
using Sas.BodySystem.Process.Models.DataTransferObject;
using Sas.BodySystem.Process.Models.Queries;

namespace Sas.BodySystem.Process.Controllers;

[ApiController]
[TypeFilter(typeof(BodySystemExceptionFilter))]
[Route("body-system")]
public class BodySystemController(IMediator mediator) : ControllerBase
{
    // TODO: Use it in memory cache
    private const string BodySystemCacheKey = "BodySystem";

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new CreateBodySystemRequest(AtTime: null), cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BodySystemConfigurationDTO config, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new CreateBodySystemRequest(config.Scale, config.AtTime, config.GravitationalConstant), cancellationToken));

    [HttpGet("{atTime}")]
    public async Task<IActionResult> GetAtTime(double atTime)
        => Ok(await mediator.Send(new CreateBodySystemRequest(AtTime: atTime), cancellationToken: default).ConfigureAwait(false));
}
