using AutoMapper;
using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Notifications;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class ChangeBodyStateHandler(IBodyRepository repository, IMapper mapper, IBodyNotificationClient notifications) : IRequestHandler<ChangeBodyStateRequest, BodyDto>
{
    public async Task<BodyDto> Handle(ChangeBodyStateRequest request, CancellationToken cancellationToken)
    {
        BodyEntity body = await repository.GetBodyByNameAsync(request.Name, cancellationToken)
            ?? throw new NoBodyInDatabaseException($"There is no body in database with name {request.Name}");
        if (body.Enabled != request.Enabled)
        {
            body.Enabled = request.Enabled;
            await repository.UpdateBodyAsync(body, cancellationToken);
        }
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return mapper.Map<BodyDto>(body);
    }
}
