using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Sas.Body.Business.Models.Domain.BodySystems;
using Sas.Body.Business.Models.Queries;

namespace Sas.Body.Business.Handlers;

public sealed class GetBodySystemHandler(IMediator mediator, IMemoryCache cache)
    : IRequestHandler<GetBodySystem, BodySystem>
{
    private const string CacheKey = "BodySystem";

    public async Task<BodySystem> Handle(GetBodySystem request, CancellationToken cancellationToken)
    {
        if (request.AtTime is null)
        {
            BodySystem created = await mediator.Send(new CreateBodySystem(SkipStaticPoints: false), cancellationToken);
            cache.Set(CacheKey, created);
            return created;
        }

        if (!cache.TryGetValue(CacheKey, out BodySystem? bodySystem) || bodySystem is null)
            bodySystem = await mediator.Send(new CreateBodySystem(SkipStaticPoints: true), cancellationToken);

        bodySystem.Move(request.AtTime.Value);
        return bodySystem;
    }
}
