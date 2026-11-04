using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sas.Body.Business.Handlers;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Business.Profiles;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Exceptions;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Notifications;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Service.Test;

public class BodyRequestHandlerTest
{
    private readonly Mock<IBodyRepository> repository = new();
    private readonly Mock<IBodyNotificationClient> notifications = new();
    private readonly IMapper mapper = new MapperConfiguration(configuration =>
        configuration.AddMaps(typeof(BodyMapper).Assembly), NullLoggerFactory.Instance).CreateMapper();

    [Fact]
    public async Task MediatorResolvesBusinessHandlersAndMapsBodies()
    {
        using var cancellation = new CancellationTokenSource();
        repository.Setup(r => r.GetAllBodiesAsync(cancellation.Token))
            .ReturnsAsync(new[] { Body("Earth") });
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton(repository.Object);
        services.AddSingleton(notifications.Object);
        services.AddSingleton(mapper);
        services.AddMemoryCache();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(BodyRequestHandler).Assembly));
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });

        var bodies = await provider.GetRequiredService<IMediator>().Send(new GetBodies(), cancellation.Token);

        Assert.Equal("Earth", Assert.Single(bodies).Name);
        repository.Verify(r => r.GetAllBodiesAsync(cancellation.Token), Times.Once);
    }

    [Fact]
    public async Task CloneResetsIdsAndNotifiesAfterSaving()
    {
        using var cancellation = new CancellationTokenSource();
        BodyEntity original = Body("Earth");
        repository.Setup(r => r.GetBodyByNameAsNoTrackingAsync("Earth", cancellation.Token)).ReturnsAsync(original);
        repository.Setup(r => r.CreateBodyAsync(It.IsAny<BodyEntity>(), cancellation.Token))
            .ReturnsAsync((BodyEntity body, CancellationToken _) => body);
        notifications.Setup(n => n.SendBodyDatabaseChangedNotification("sender", cancellation.Token))
            .Callback(() => repository.Verify(r => r.CreateBodyAsync(original, cancellation.Token), Times.Once))
            .Returns(Task.CompletedTask);

        BodyEntity? clone = await Handler().Handle(new CloneBody("Earth", "sender"), cancellation.Token);

        Assert.NotNull(clone);
        Assert.Equal("EarthCopy", clone.Name);
        Assert.Equal(0, clone.Id);
        Assert.Equal(0, clone.Position.Id);
        Assert.Equal(0, clone.Velocity.Id);
        notifications.Verify(n => n.SendBodyDatabaseChangedNotification("sender", cancellation.Token), Times.Once);
    }

    [Fact]
    public async Task ExistingNameDoesNotSaveOrNotify()
    {
        repository.Setup(r => r.GetBodyByNameAsync("Earth", It.IsAny<CancellationToken>())).ReturnsAsync(Body("Earth"));

        await Assert.ThrowsAsync<BodyAlreadyExistsException>(() => Handler().Handle(
            new CreateBody(new NewBodyDto { Name = "Earth" }, null), CancellationToken.None));

        repository.Verify(r => r.CreateBodyAsync(It.IsAny<BodyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        notifications.Verify(n => n.SendBodyDatabaseChangedNotification(It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DefaultsCreateOnlySupportedBodiesMissingFromDatabase()
    {
        repository.Setup(r => r.GetAllBodiesNamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new[] { "Sun" });
        repository.Setup(r => r.CreateRangeBodyAsync(It.IsAny<List<BodyEntity>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<BodyEntity> bodies, CancellationToken _) => bodies);

        var bodies = await Handler().Handle(new CreateDefaultBodies(["Sun", "Earth", "Earth", "Unknown"], "sender"), CancellationToken.None);

        Assert.Equal("Earth", Assert.Single(bodies).Name);
        notifications.Verify(n => n.SendBodyDatabaseChangedNotification("sender", CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task FailedSaveDoesNotSendNotification()
    {
        repository.Setup(r => r.CreateBodyAsync(It.IsAny<BodyEntity>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Save failed"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => Handler().Handle(
            new CreateBody(new NewBodyDto { Name = "Earth" }, null), CancellationToken.None));

        notifications.Verify(n => n.SendBodyDatabaseChangedNotification(It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task MissingBodyCannotBeUpdated()
    {
        await Assert.ThrowsAsync<NoBodyInDatabaseException>(() => Handler().Handle(
            new UpdateBody(new BodyDto { Id = 42, Name = "Earth" }, null), CancellationToken.None));

        repository.Verify(r => r.UpdateBodyAsync(It.IsAny<BodyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        notifications.Verify(n => n.SendBodyDatabaseChangedNotification(It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private BodyRequestHandler Handler() => new(repository.Object, notifications.Object, mapper);

    private static BodyEntity Body(string name) => new()
    {
        Id = 42,
        Name = name,
        Position = new VectorEntity { Id = 43, X = 1 },
        Velocity = new VectorEntity { Id = 44, Y = 1 }
    };
}
