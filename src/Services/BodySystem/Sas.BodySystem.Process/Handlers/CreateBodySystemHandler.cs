using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.BodySystem.Process.Models.DataTransferObject;
using Sas.BodySystem.Process.Models.Domain.Bodies;
using Sas.BodySystem.Process.Models.Queries;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits;

namespace Sas.BodySystem.Process.Handlers;


public sealed class CreateBodySystemHandler(IMediator mediator, IPositionedOrbitFactory orbitFactory) : IRequestHandler<CreateBodySystemRequest, BodySystemDto>
{

    public async Task<BodySystemDto> Handle(CreateBodySystemRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<BodyDomain> enabledBodies = await GetEnabledBodies(cancellationToken);
        BodySystems.BodySystem system = new(enabledBodies, orbitFactory, request.GravitationalConstant);
        system.UpdateBodySystem(request.Scale, cancellationToken);
        return new BodySystemDto()
        {
            GravitationalConstant = system.GravitationalConstant,
            Barycentrum = system.Barycentrum,
            Bodies = system.Bodies,
            Orbits = system.Orbits
        };
    }

    private async Task<IEnumerable<BodyDomain>> GetEnabledBodies(CancellationToken cancellationToken)
    {
        IEnumerable<BodyDto> bodies = await mediator.Send(new GetEnabledBodiesRequest(), cancellationToken);
        IEnumerable<BodyDomain> enabledBodies = bodies.Where(b => b?.Enabled == true).Select(b => new BodyDomain
        {
            Name = b?.Name ?? throw new InvalidOperationException("Body name is missing."),
            Mass = b?.Mass ?? throw new InvalidOperationException("Body mass is missing."),
            Position = b?.Position is { } p ? new Vector(p.X, p.Y, p.Z) : throw new InvalidOperationException("Body position is missing."),
            Velocity = b?.Velocity is { } v ? new Vector(v.X, v.Y, v.Z) : throw new InvalidOperationException("Body velocity is missing."),
            Radius = b?.Radius ?? 0
        });
        return enabledBodies;
    }
}
