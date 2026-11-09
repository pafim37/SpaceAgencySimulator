using MediatR;
using Sas.BodySystem.Service.Models.Queries;

namespace Sas.BodySystem.Service.Handlers;

public sealed class GetBodySystemHandler(IMediator mediator) : IRequestHandler<GetBodySystem, BodySystems.BodySystem>
{
    public async Task<BodySystems.BodySystem> Handle(GetBodySystem request, CancellationToken cancellationToken)
    {
        if (!double.IsFinite(request.AtTime) || request.AtTime < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.AtTime));
        }
        BodySystems.BodySystem system = await mediator.Send(new CreateBodySystem(), cancellationToken);
        return system;
    }
}
