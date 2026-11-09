using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.BodySystem.Service.Models.Queries;
using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Service.Handlers;

using Body = Sas.BodySystem.Service.Models.Domain.Bodies.Body;

public sealed class CreateBodySystemHandler(IMediator mediator) : IRequestHandler<CreateBodySystem, BodySystems.BodySystem>
{
    public async Task<BodySystems.BodySystem> Handle(CreateBodySystem request, CancellationToken cancellationToken)
    {
        IEnumerable<BodyDto> bodies = await mediator.Send(new GetBodies(), cancellationToken);
        IEnumerable<Body> enabledBodies = bodies.Where(b => b.Enabled == true).Select(b => new Body(
            b.Name ?? throw new InvalidOperationException("Body name is missing."),
            b.Mass ?? throw new InvalidOperationException("Body mass is missing."),
            b.Position is { } p ? new Vector(p.X, p.Y, p.Z) : throw new InvalidOperationException("Body position is missing."),
            b.Velocity is { } v ? new Vector(v.X, v.Y, v.Z) : throw new InvalidOperationException("Body velocity is missing."),
            b.Radius ?? 0));
        BodySystems.BodySystem system = new(mediator, enabledBodies);
        system.UpdateBodySystem();
        await system.FindOrbits().ConfigureAwait(false);
        return system;
    }
}
