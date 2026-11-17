using AutoMapper;
using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Notifications;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class CloneBodyHandler(IBodyRepository repository, IMapper mapper, IBodyNotificationClient notifications) : IRequestHandler<CloneBodyRequest, BodyDto>
{
    public async Task<BodyDto> Handle(CloneBodyRequest request, CancellationToken cancellationToken)
    {
        BodyEntity? body = await repository.GetBodyByNameAsNoTrackingAsync(request.Name, cancellationToken)
            ?? throw new NoBodyInDatabaseException($"Cannot clone body. Body with name {request.Name} not found");
        BodyEntity clonedBody = new()
        {
            Name = body.Name + "Copy",
            Mass = body.Mass,
            Radius = body.Radius,
            Enabled = body.Enabled,
            Position = new VectorEntity
            {
                X = body.Position.X,
                Y = body.Position.Y,
                Z = body.Position.Z
            },
            Velocity = new VectorEntity
            {
                X = body.Velocity.X,
                Y = body.Velocity.Y,
                Z = body.Velocity.Z
            }
        };
        await EnsureNameAvailable(clonedBody.Name, cancellationToken);
        BodyEntity created = await repository.CreateBodyAsync(clonedBody, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return mapper.Map<BodyDto>(created);
    }

    private async Task EnsureNameAvailable(string name, CancellationToken cancellationToken)
    {
        if (await repository.GetBodyByNameAsync(name, cancellationToken) is not null)
        {
            throw new BodyAlreadyExistsException($"Body with name {name} already exists");
        }
    }
}
