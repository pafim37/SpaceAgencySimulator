using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Repositories;
using Sas.Body.Core.Notifications;

namespace Sas.Body.Business.Handlers;

public sealed class CloneBodyHandler(IBodyRepository repository, IBodyNotificationClient notifications) : IRequestHandler<CloneBody, BodyEntity?>
{
    public async Task<BodyEntity?> Handle(CloneBody request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Name);
        BodyEntity? body = await repository.GetBodyByNameAsNoTrackingAsync(request.Name, cancellationToken);
        if (body is null)
        {
            return null;
        }

        body.Id = 0;
        body.Name = request.Name + "Copy";
        body.Position.Id = 0;
        body.Velocity.Id = 0;
        await EnsureNameAvailable(body.Name, cancellationToken);
        BodyEntity created = await repository.CreateBodyAsync(body, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return created;
    }

    private async Task EnsureNameAvailable(string name, CancellationToken cancellationToken)
    {
        if (await repository.GetBodyByNameAsync(name, cancellationToken) is not null)
        {
            throw new BodyAlreadyExistsException($"Body with name {name} already exists");
        }
    }
}
