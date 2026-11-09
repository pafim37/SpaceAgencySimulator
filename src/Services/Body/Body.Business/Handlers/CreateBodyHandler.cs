using MediatR;
using Sas.Body.Business.Models.Queries;
using AutoMapper;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Repositories;
using Sas.Body.Core.Notifications;

namespace Sas.Body.Business.Handlers;

public sealed class CreateBodyHandler(IBodyRepository repository, IBodyNotificationClient notifications, IMapper mapper) : IRequestHandler<CreateBody, BodyEntity>
{
    public async Task<BodyEntity> Handle(CreateBody request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Body.Name);
        BodyEntity body = mapper.Map<BodyEntity>(request.Body);
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
