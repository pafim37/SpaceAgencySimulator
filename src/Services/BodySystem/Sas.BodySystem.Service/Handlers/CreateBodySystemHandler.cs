using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.BodySystem.Process.Models.Queries;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits;

namespace Sas.BodySystem.Process.Handlers;

using Body = Models.Domain.Bodies.Body;

public sealed class CreateBodySystemHandler(IMediator mediator, IPositionedOrbitFactory orbitFactory) : IRequestHandler<CreateBodySystem, BodySystems.BodySystem>
{
    public async Task<BodySystems.BodySystem> Handle(CreateBodySystem request, CancellationToken cancellationToken)
    {
        IEnumerable<Body> enabledBodies = await GetEnabledBodies(cancellationToken);
        BodySystems.BodySystem system = new(enabledBodies, orbitFactory, request.GravitationalConstant);
        system.UpdateBodySystem(cancellationToken);
        return system;
    }

    private async Task<IEnumerable<Body>> GetEnabledBodies(CancellationToken cancellationToken)
    {
        IEnumerable<BodyDto> bodies = await mediator.Send(new GetEnabledBodiesRequest(), cancellationToken);
        IEnumerable<Body> enabledBodies = bodies.Where(b => b?.Enabled == true).Select(b => new Body(
            b?.Name ?? throw new InvalidOperationException("Body name is missing."),
            b?.Mass ?? throw new InvalidOperationException("Body mass is missing."),
            b?.Position is { } p ? new Vector(p.X, p.Y, p.Z) : throw new InvalidOperationException("Body position is missing."),
            b?.Velocity is { } v ? new Vector(v.X, v.Y, v.Z) : throw new InvalidOperationException("Body velocity is missing."),
            b?.Radius ?? 0));
        return enabledBodies;
    }
}
