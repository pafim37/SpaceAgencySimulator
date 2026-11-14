using MediatR;
using Sas.Body.Business.Models.Queries;
using AutoMapper;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Repositories;
using Sas.Body.Core.Notifications;

namespace Sas.Body.Business.Handlers;

public sealed class CreateDefaultBodiesHandler(IBodyRepository repository, IBodyNotificationClient notifications, IMapper mapper) : IRequestHandler<CreateDefaultBodiesRequest, List<BodyEntity>>
{
    public async Task<List<BodyEntity>> Handle(CreateDefaultBodiesRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<string> currentNames = await repository.GetAllBodiesNamesAsync(cancellationToken);
        List<string> names = [.. SupportedBodyNames.All.Intersect(request.Names.Except(currentNames))];
        if (names.Count == 0)
        {
            throw new BodyAlreadyExistsException("The request does not contain any supported body name or supported body already exists");
        }

        List<BodyEntity> bodies = mapper.Map<List<BodyEntity>>(DefaultBodyFactory.Create(names));
        List<BodyEntity> created = await repository.CreateRangeBodyAsync(bodies, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return created;
    }
}
