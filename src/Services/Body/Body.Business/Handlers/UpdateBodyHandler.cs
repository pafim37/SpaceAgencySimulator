using AutoMapper;
using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Notifications;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class UpdateBodyHandler(IBodyRepository repository, IBodyNotificationClient notifications, IMapper mapper) : IRequestHandler<UpdateBodyRequest, BodyDto>
{
    public async Task<BodyDto> Handle(UpdateBodyRequest request, CancellationToken cancellationToken)
    {
        BodyPatchDto update = request.Body;
        BodyEntity body = await repository.GetBodyById(update.Id, cancellationToken)
            ?? throw new NoBodyInDatabaseException($"There is no body in database with id {update.Id}");
        body.Name = update.Name ?? body.Name;
        body.Mass = update.Mass ?? body.Mass;
        body.Position = mapper.Map<VectorEntity>(update.Position) ?? body.Position;
        body.Velocity = mapper.Map<VectorEntity>(update.Velocity) ?? body.Velocity;
        body.Radius = update.Radius ?? body.Radius;
        body.Enabled = update.Enabled ?? body.Enabled;
        BodyEntity updated = await repository.UpdateBodyAsync(body, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return mapper.Map<BodyDto>(updated);
    }
}
