using AutoMapper;
using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Notifications;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class DeleteBodyHandler(IBodyRepository repository, IMapper mapper, IBodyNotificationClient notifications) : IRequestHandler<DeleteBodyRequest, BodyDto>
{
    public async Task<BodyDto> Handle(DeleteBodyRequest request, CancellationToken cancellationToken)
    {
        BodyEntity deleted = await repository.DeleteBodyAsync(request.Name, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return mapper.Map<BodyDto>(deleted);
    }
}
