using AutoMapper;
using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Notifications;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class CreateBodyHandler(IBodyRepository repository, IBodyNotificationClient notifications, IMapper mapper) : IRequestHandler<CreateBodyRequest, BodyDto>
{
    public async Task<BodyDto> Handle(CreateBodyRequest request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Body.Name);
        BodyEntity body = mapper.Map<BodyEntity>(request.Body);
        await EnsureNameAvailable(body.Name, cancellationToken);
        BodyEntity created = await repository.CreateBodyAsync(body, cancellationToken);
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
