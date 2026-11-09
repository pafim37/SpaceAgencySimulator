using MediatR;
using Sas.Body.Business.Models.Queries;
using AutoMapper;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Repositories;
using Sas.Body.Core.Notifications;

namespace Sas.Body.Business.Handlers;

public sealed class UpdateBodyHandler(IBodyRepository repository, IBodyNotificationClient notifications, IMapper mapper) : IRequestHandler<UpdateBody, BodyEntity>
{
    public async Task<BodyEntity> Handle(UpdateBody request, CancellationToken cancellationToken)
    {
        BodyDto update = request.Body;
        ArgumentException.ThrowIfNullOrWhiteSpace(update.Name);
        ArgumentNullException.ThrowIfNull(update.Id);
        BodyEntity body = await repository.GetBodyById(update.Id.Value, cancellationToken)
            ?? throw new NoBodyInDatabaseException($"There is no body in database with name {update.Name}");
        body.Name = update.Name;
        body.Mass = update.Mass ?? body.Mass;
        body.Position = mapper.Map<VectorEntity>(update.Position) ?? body.Position;
        body.Velocity = mapper.Map<VectorEntity>(update.Velocity) ?? body.Velocity;
        body.Radius = update.Radius ?? body.Radius;
        BodyEntity updated = await repository.UpdateBodyAsync(body, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return updated;
    }
}
