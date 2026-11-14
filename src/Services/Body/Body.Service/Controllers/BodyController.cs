using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;

namespace Sas.Body.Service.Controllers;

[ApiController]
[Route("body")]
public class BodyController(IMediator mediator) : ControllerBase
{
    private const string SignalRClientIdHeaderName = "X-SAS-SignalRClientId";

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetBodiesRequest(), cancellationToken));

    [HttpGet("names")]
    public async Task<IActionResult> GetAllNames(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetBodyNamesRequest(), cancellationToken));

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
    {
        BodyDto? body = await mediator.Send(new GetBodyByNameRequest(name), cancellationToken);
        return body is null ? NotFound() : Ok(body);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NewBodyDto body,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
        => Created("/", await mediator.Send(new CreateBodyRequest(body, signalRConnectionId), cancellationToken));

    [HttpPost("clone/{bodyName}")]
    public async Task<IActionResult> Clone(string bodyName,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
    {
        var body = await mediator.Send(new CloneBodyRequest(bodyName, signalRConnectionId), cancellationToken);
        return body is null ? NotFound() : Created("/", body);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromBody] BodyDto body,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new UpdateBodyRequest(body, signalRConnectionId), cancellationToken));

    [HttpPost("{name}")]
    public async Task<IActionResult> ChangeState(string name, [FromBody] bool newState,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ChangeBodyStateRequest(name, newState, signalRConnectionId), cancellationToken));

    [HttpDelete("{name}")]
    public async Task<IActionResult> Delete(string name,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new DeleteBodyRequest(name, signalRConnectionId), cancellationToken));

    [HttpGet("supported-names")]
    public async Task<IActionResult> GetSupportedNames(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetSupportedBodyNamesRequest(), cancellationToken));

    [HttpPost("defaults")]
    public async Task<IActionResult> Create([FromBody] List<string> bodynames,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
        => Created("/", await mediator.Send(new CreateDefaultBodiesRequest(bodynames, signalRConnectionId), cancellationToken));
}
