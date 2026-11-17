using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.Body.Business;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Repositories;
using Sas.Body.Service.Filters;

namespace Sas.Body.Service.Controllers;

[ApiController]
[TypeFilter(typeof(BodyExceptionFilter))]
[Route("body")]
public class BodyController(IMediator mediator, IBodyRepository bodyRepository, IMapper mapper) : ControllerBase
{
    private const string SignalRClientIdHeaderName = "X-SAS-SignalRClientId";

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(mapper.Map<IEnumerable<BodyDto>>(await bodyRepository.GetAllBodiesAsync(cancellationToken).ConfigureAwait(false)));

    [HttpGet("names")]
    public async Task<IActionResult> GetAllNames(CancellationToken cancellationToken)
        => Ok(mapper.Map<string[]>(await bodyRepository.GetAllBodiesNamesAsync(cancellationToken)));

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
    {
        BodyEntity? body = await bodyRepository.GetBodyByNameAsNoTrackingAsync(name, cancellationToken);
        return body is null ? NotFound() : Ok(mapper.Map<BodyDto>(body));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NewBodyDto body,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
    {
        BodyDto newbody = await mediator.Send(new CreateBodyRequest(body, signalRConnectionId), cancellationToken);
        return Created("/", newbody);
    }

    [HttpPost("clone/{bodyName}")]
    public async Task<IActionResult> Clone(string bodyName,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
    {
        BodyDto? body = await mediator.Send(new CloneBodyRequest(bodyName, signalRConnectionId), cancellationToken);
        return body is null ? NotFound() : Created("/", body);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch([FromBody] BodyPatchDto body,
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
    public async Task<IActionResult> GetSupportedNames()
        => Ok(SupportedBodyNames.All);

    [HttpPost("defaults")]
    public async Task<IActionResult> Create([FromBody] List<string> bodynames,
        [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId, CancellationToken cancellationToken)
        => Created("/", await mediator.Send(new CreateDefaultBodiesRequest(bodynames, signalRConnectionId), cancellationToken));
}
