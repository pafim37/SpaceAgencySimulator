using AutoMapper;
using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Notifications;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class BodyRequestHandler(IBodyRepository repository, IBodyNotificationClient notifications, IMapper mapper) :
    IRequestHandler<GetBodies, IEnumerable<BodyDto>>,
    IRequestHandler<GetBodyNames, IEnumerable<string>>,
    IRequestHandler<GetBodyByName, BodyDto?>,
    IRequestHandler<GetSupportedBodyNames, IReadOnlyList<string>>,
    IRequestHandler<CreateBody, BodyEntity>,
    IRequestHandler<CloneBody, BodyEntity?>,
    IRequestHandler<UpdateBody, BodyEntity>,
    IRequestHandler<ChangeBodyState, BodyEntity>,
    IRequestHandler<DeleteBody, BodyEntity>,
    IRequestHandler<CreateDefaultBodies, List<BodyEntity>>
{
    private static readonly IReadOnlyList<string> SupportedNames = Array.AsReadOnly(
        ["Sun", "Mercury", "Wenus", "Earth", "Mars", "Moon", "Player"]);

    public async Task<IEnumerable<BodyDto>> Handle(GetBodies request, CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<BodyDto>>(await repository.GetAllBodiesAsync(cancellationToken));

    public Task<IEnumerable<string>> Handle(GetBodyNames request, CancellationToken cancellationToken)
        => repository.GetAllBodiesNamesAsync(cancellationToken);

    public async Task<BodyDto?> Handle(GetBodyByName request, CancellationToken cancellationToken)
        => mapper.Map<BodyDto?>(await repository.GetBodyByNameAsync(request.Name, cancellationToken));

    public Task<IReadOnlyList<string>> Handle(GetSupportedBodyNames request, CancellationToken cancellationToken)
        => Task.FromResult(SupportedNames);

    public async Task<BodyEntity> Handle(CreateBody request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Body.Name);
        BodyEntity body = mapper.Map<BodyEntity>(request.Body);
        await EnsureNameAvailable(body.Name, cancellationToken);
        BodyEntity created = await repository.CreateBodyAsync(body, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return created;
    }

    public async Task<BodyEntity?> Handle(CloneBody request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Name);
        BodyEntity? body = await repository.GetBodyByNameAsNoTrackingAsync(request.Name, cancellationToken);
        if (body is null) return null;
        body.Id = 0;
        body.Name = request.Name + "Copy";
        body.Position.Id = 0;
        body.Velocity.Id = 0;
        await EnsureNameAvailable(body.Name, cancellationToken);
        BodyEntity created = await repository.CreateBodyAsync(body, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return created;
    }

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

    public async Task<BodyEntity> Handle(ChangeBodyState request, CancellationToken cancellationToken)
    {
        BodyEntity body = await repository.GetBodyByNameAsync(request.Name, cancellationToken)
            ?? throw new NoBodyInDatabaseException($"There is no body in database with name {request.Name}");
        if (body.Enabled != request.Enabled)
        {
            body.Enabled = request.Enabled;
            await repository.UpdateBodyAsync(body, cancellationToken);
        }
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return body;
    }

    public async Task<BodyEntity> Handle(DeleteBody request, CancellationToken cancellationToken)
    {
        BodyEntity deleted = await repository.DeleteBodyAsync(request.Name, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return deleted;
    }

    public async Task<List<BodyEntity>> Handle(CreateDefaultBodies request, CancellationToken cancellationToken)
    {
        IEnumerable<string> currentNames = await repository.GetAllBodiesNamesAsync(cancellationToken);
        List<string> names = [.. SupportedNames.Intersect(request.Names.Except(currentNames))];
        if (names.Count == 0)
            throw new BodyAlreadyExistsException("The request does not contain any supported body name or supported body already exists");
        List<BodyEntity> bodies = mapper.Map<List<BodyEntity>>(DefaultBodyFactory.Create(names));
        List<BodyEntity> created = await repository.CreateRangeBodyAsync(bodies, cancellationToken);
        await notifications.SendBodyDatabaseChangedNotification(request.SenderId, cancellationToken);
        return created;
    }

    private async Task EnsureNameAvailable(string name, CancellationToken cancellationToken)
    {
        if (await repository.GetBodyByNameAsync(name, cancellationToken) is not null)
            throw new BodyAlreadyExistsException($"Body with name {name} already exists");
    }
}
