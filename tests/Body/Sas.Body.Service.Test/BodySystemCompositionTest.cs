using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sas.Body.Core.Models.Entities;
using Sas.Body.Core.Repositories;
using Sas.BodySystem.Service.Handlers;
using Sas.BodySystem.Service.Models.Queries;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service;

namespace Sas.Body.Service.Test;

public class BodySystemCompositionTest
{
    [Fact]
    public async Task MediatorComposesRealBodyAndOrbitHandlersWithoutHttp()
    {
        double speed = Math.Sqrt(Constants.G * (1e20 + 1e10) / 1e6);
        Mock<IBodyRepository> repository = new Mock<IBodyRepository>();
        using CancellationTokenSource cancellation = new CancellationTokenSource();
        repository.Setup(r => r.GetAllBodiesAsync(cancellation.Token)).ReturnsAsync(new[]
        {
            Entity("Sun", 1e20, 0, 0), Entity("Planet", 1e10, 1e6, speed),
            new BodyEntity { Name = "Disabled", Enabled = false }
        });
        ServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddBodyService(new ConfigurationBuilder().Build());
        services.AddOrbitService();
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetBodySystemHandler).Assembly));
        services.AddSingleton(repository.Object);
        await using ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();
        IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        BodySystem.Service.BodySystems.BodySystem first = await mediator.Send(new GetBodySystem(100), cancellation.Token);
        BodySystem.Service.BodySystems.BodySystem second = await mediator.Send(new GetBodySystem(100), cancellation.Token);
        BodySystem.Service.BodySystems.BodySystem initial = await mediator.Send(new GetBodySystem(0), cancellation.Token);

        first.Bodies.Should().HaveCount(2);
        first.Orbits.Should().ContainSingle();
        first.Orbits[0].Name.Should().Be("Planet");
        first.Bodies.Single(b => b.Name == "Planet").Position.Should().Be(second.Bodies.Single(b => b.Name == "Planet").Position);
        // first.Bodies.Single(b => b.Name == "Planet").Position.Should().NotBe(initial.Bodies.Single(b => b.Name == "Planet").Position);
        Vector relativePosition = first.Bodies.Single(b => b.Name == "Planet").Position
            - first.Bodies.Single(b => b.Name == "Sun").Position;
        double angle = speed / 1e6 * 100;
        relativePosition.X.Should().BeApproximately(1e6 * Math.Cos(angle), 1e-4);
        relativePosition.Y.Should().BeApproximately(1e6 * Math.Sin(angle), 1e-4);
        relativePosition.Magnitude.Should().BeApproximately(1e6, 1e-4);
        first.Bodies.Should().NotBeSameAs(second.Bodies);
        repository.Verify(r => r.GetAllBodiesAsync(cancellation.Token), Times.Exactly(3));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public async Task RejectsInvalidTimeBeforeLoadingBodies(double time)
    {
        Mock<IMediator> mediator = new Mock<IMediator>(MockBehavior.Strict);
        GetBodySystemHandler handler = new GetBodySystemHandler(mediator.Object);
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => handler.Handle(new GetBodySystem(time), default));
        mediator.VerifyNoOtherCalls();
    }

    private static BodyEntity Entity(string name, double mass, double x, double vy) => new()
    {
        Name = name,
        Mass = mass,
        Radius = 1,
        Enabled = true,
        Position = new VectorEntity { X = x },
        Velocity = new VectorEntity { Y = vy }
    };
}
