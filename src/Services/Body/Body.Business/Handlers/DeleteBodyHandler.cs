using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Repositories;
using Sas.Body.Core.Notifications;

namespace Sas.Body.Business.Handlers;

public sealed class DeleteBodyHandler(IBodyRepository repository, IBodyNotificationClient notifications) : IRequestHandler<DeleteBodyRequest, BodyEntity>
{
    public async Task<BodyEntity> Handle(DeleteBodyRequest request, CancellationToken cancellationToken)
    {
        BodyEntity deleted = await repository.DeleteBodyAsync(request.Name, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return deleted;
    }
}
