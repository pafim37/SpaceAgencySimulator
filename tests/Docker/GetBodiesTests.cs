using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Models.Entities;
using Sas.BodySystem.Process.Models.DataTransferObject;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using System.Net;
using System.Net.Http.Json;
using Xunit;
namespace Sas.Docker.Tests;

[Collection("Docker")]
public sealed class GetBodiesTests(DockerFixture fixture)
{
    [Fact]
    public async Task GetBody_ReturnsSeededBodies()
    {
        // Act
        using HttpResponseMessage response = await fixture.Client.GetAsync("/body");
        List<BodyEntity>? bodySystem = await response.Content.ReadFromJsonAsync<List<BodyEntity>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(bodySystem);
        Assert.Contains("Sun", bodySystem.Select(body => body.Name));
    }

    [Fact]
    public async Task BodySystem_ReturnsBodySystem()
    {
        // Arrange
        string[] names = ["Earth", "Moon"];
        using HttpResponseMessage responseDefaults = await fixture.Client.PostAsJsonAsync("/body/defaults", names);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

        using HttpResponseMessage enableSun = await fixture.Client.PostAsJsonAsync("/body/Sun", true);
        Assert.Equal(HttpStatusCode.OK, enableSun.StatusCode);

        // Act
        using HttpResponseMessage response = await fixture.Client.GetAsync("/body-system");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        BodySystemDto? bodySystem = await response.Content.ReadFromJsonAsync<BodySystemDto>();

        // Gravitational Constant
        Assert.NotNull(bodySystem);
        Assert.Equal(Constants.G, bodySystem.GravitationalConstant);

        // Barycentrum
        Assert.NotNull(bodySystem.Barycentrum);
        Assert.Equal("Barycentrum", bodySystem.Barycentrum.Name);
        Assert.Equal(Vector.Zero, bodySystem.Barycentrum.Position);
        Assert.Equal(Vector.Zero, bodySystem.Barycentrum.Velocity);
        Assert.Equal(0, bodySystem.Barycentrum.Radius);
        Assert.Null(bodySystem.Barycentrum.ParentName);
        Assert.Null(bodySystem.Barycentrum.SphereOfInfluenceRadius);

        // Bodies
        Assert.NotEmpty(bodySystem.Bodies);
        Assert.Contains(bodySystem.Bodies, body => body.Name == "Sun");
        Assert.Contains(bodySystem.Bodies, body => body.Name == "Earth");
        Assert.Contains(bodySystem.Bodies, body => body.Name == "Moon");
        Assert.Null(bodySystem.Bodies.First(body => body.Name == "Sun").ParentName);
        Assert.Equal("Sun", bodySystem.Bodies.First(body => body.Name == "Earth").ParentName);
        Assert.Equal("Earth", bodySystem.Bodies.First(body => body.Name == "Moon").ParentName);

        // Orbits
        Assert.NotEmpty(bodySystem.Orbits);
        Assert.Equal(2, bodySystem.Orbits.Count);
        Assert.Collection(bodySystem.Orbits,
            orbit1 => Assert.Equal("Earth", orbit1.Name),
            orbit2 => Assert.Equal("Moon", orbit2.Name)
        );

        // Delete the created bodies
        await DeleteBodies(names);
    }

    [Fact]
    public async Task GetBodiesNames_ReturnsBodiesNames()
    {
        // Arrange
        string[] names = ["Earth", "Moon"];
        using HttpResponseMessage responseDefaults = await fixture.Client.PostAsJsonAsync("/body/defaults", names);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

        // Act
        using HttpResponseMessage response = await fixture.Client.GetAsync("/body/names");
        List<string>? bodyNames = await response.Content.ReadFromJsonAsync<List<string>>();

        // Assert
        Assert.NotNull(bodyNames);
        Assert.Contains("Sun", bodyNames);
        Assert.Contains("Earth", bodyNames);
        Assert.Contains("Moon", bodyNames);

        // Delete the created bodies
        await DeleteBodies(names);
    }

    [Fact]
    public async Task GetBodiesName_ReturnsBody()
    {
        // Arrange
        string[] names = ["Earth", "Moon"];
        using HttpResponseMessage responseDefaults = await fixture.Client.PostAsJsonAsync("/body/defaults", names);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

        // Act
        using HttpResponseMessage response = await fixture.Client.GetAsync("/body/Earth");
        BodyDto? body = await response.Content.ReadFromJsonAsync<BodyDto>();

        // Assert
        Assert.NotNull(body);
        Assert.Equal("Earth", body.Name);

        // Delete the created bodies
        await DeleteBodies(names);
    }

    // TODO: test also validation
    [Fact]
    public async Task CreateBody_CreatesBody()
    {
        // Arrange
        string name = "Planet";
        NewBodyDto newBody = new()
        {
            Name = name,
            Mass = 1.0,
            Radius = 1.0,
            Enabled = true,
            Position = new VectorDto { X = 100, Y = 0, Z = 0 },
            Velocity = new VectorDto { X = 0, Y = 6, Z = 0 }
        };
        using HttpResponseMessage responseDefaults = await fixture.Client.PostAsJsonAsync("/body", newBody);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

        // Act
        using HttpResponseMessage response = await fixture.Client.GetAsync("/body/Planet");
        BodyDto? body = await response.Content.ReadFromJsonAsync<BodyDto>();

        // Assert
        Assert.NotNull(body);
        Assert.Equal("Planet", body.Name);
        Assert.Equal(1.0, body.Mass);
        Assert.Equal(1.0, body.Radius);
        Assert.True(body.Enabled);
        Assert.Equal(100.0, body.Position!.X);
        Assert.Equal(0.0, body.Position.Y);
        Assert.Equal(0.0, body.Position.Z);
        Assert.Equal(0.0, body.Velocity!.X);
        Assert.Equal(6.0, body.Velocity.Y);
        Assert.Equal(0.0, body.Velocity.Z);

        // Delete the created bodies
        await DeleteBodies([name]);
    }

    [Fact]
    public async Task CloneBody_ClonesBody()
    {
        // Arrange
        string name = "Planet";
        NewBodyDto newBody = new()
        {
            Name = name,
            Mass = 1.0,
            Radius = 1.0,
            Enabled = true,
            Position = new VectorDto { X = 100, Y = 0, Z = 0 },
            Velocity = new VectorDto { X = 0, Y = 6, Z = 0 }
        };
        using HttpResponseMessage responseDefaults = await fixture.Client.PostAsJsonAsync("/body", newBody);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);
        // Act
        using HttpResponseMessage responseClone = await fixture.Client.PostAsJsonAsync($"/body/clone/{name}", new object());
        Assert.Equal(HttpStatusCode.Created, responseClone.StatusCode);
        BodyDto? clonedBody = await responseClone.Content.ReadFromJsonAsync<BodyDto>();
        // Assert
        Assert.NotNull(clonedBody);
        Assert.Equal(name + "Copy", clonedBody.Name);
        Assert.Equal(1.0, clonedBody.Mass);
        Assert.Equal(1.0, clonedBody.Radius);
        Assert.True(clonedBody.Enabled);
        Assert.Equal(100.0, clonedBody.Position!.X);
        Assert.Equal(0.0, clonedBody.Position.Y);
        Assert.Equal(0.0, clonedBody.Position.Z);
        Assert.Equal(0.0, clonedBody.Velocity!.X);
        Assert.Equal(6.0, clonedBody.Velocity.Y);
        Assert.Equal(0.0, clonedBody.Velocity.Z);
        // Delete the created bodies
        await DeleteBodies([name, name + "Copy"]);
    }

    // TODO: improve this test
    [Fact]
    public async Task PatchBodies_UpdateBody()
    {
        // Arrange
        string name = "Planet";
        NewBodyDto newBody = new()
        {
            Name = name,
            Mass = 1.0,
            Radius = 1.0,
            Enabled = true,
            Position = new VectorDto { X = 100, Y = 0, Z = 0 },
            Velocity = new VectorDto { X = 0, Y = 6, Z = 0 }
        };
        NewBodyDto bodyPatch = new()
        {
            Name = name,
            Mass = 2.0,
            Radius = 2.0,
            Enabled = false,
        };
        using HttpResponseMessage responseDefaults = await fixture.Client.PostAsJsonAsync("/body", newBody);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);
        BodyDto createdBody = await responseDefaults.Content.ReadFromJsonAsync<BodyDto>()
            ?? throw new InvalidOperationException("The created body response did not contain a body.");
        // Act
        BodyDto patch = new()
        {
            Id = createdBody.Id,
            Name = bodyPatch.Name,
            Mass = bodyPatch.Mass,
            Radius = bodyPatch.Radius,
            Enabled = bodyPatch.Enabled
        };
        using HttpResponseMessage responsePatch = await fixture.Client.SendAsync(new HttpRequestMessage(HttpMethod.Patch, "/body")
        {
            Content = JsonContent.Create(patch)
        });
        Assert.Equal(HttpStatusCode.OK, responsePatch.StatusCode);
        using HttpResponseMessage responseGetBody = await fixture.Client.GetAsync($"/body/{name}");
        BodyDto? bodyAfterPatch = await responseGetBody.Content.ReadFromJsonAsync<BodyDto>();
        // Assert
        Assert.NotNull(bodyAfterPatch);
        Assert.Equal(2.0, bodyAfterPatch.Mass);
        Assert.Equal(2.0, bodyAfterPatch.Radius);
        Assert.False(bodyAfterPatch.Enabled);
        // Delete the created bodies
        await DeleteBodies([name]);
    }

    [Fact]
    public async Task GetSupportedNames_ReturnsNames()
    {
        // Act
        using HttpResponseMessage response = await fixture.Client.GetAsync("/body/supported-names");
        List<string>? supportedNames = await response.Content.ReadFromJsonAsync<List<string>>();
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(supportedNames);
        Assert.Contains("Sun", supportedNames);
        Assert.Contains("Earth", supportedNames);
        Assert.Contains("Moon", supportedNames);
    }

    [Fact]
    public async Task ChangeState_ChangeBodyVisible()
    {
        // Arrange
        string name = "Planet";
        NewBodyDto newBody = new()
        {
            Name = name,
            Mass = 1.0,
            Radius = 1.0,
            Enabled = true,
            Position = new VectorDto { X = 100, Y = 0, Z = 0 },
            Velocity = new VectorDto { X = 0, Y = 6, Z = 0 }
        };
        using HttpResponseMessage responseDefaults = await fixture.Client.PostAsJsonAsync("/body", newBody);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);
        // Act
        using HttpResponseMessage responseChangeState = await fixture.Client.PostAsJsonAsync($"/body/{name}", false);
        Assert.Equal(HttpStatusCode.OK, responseChangeState.StatusCode);
        using HttpResponseMessage responseGetBody = await fixture.Client.GetAsync($"/body/{name}");
        BodyDto? bodyAfterChangeState = await responseGetBody.Content.ReadFromJsonAsync<BodyDto>();
        // Assert
        Assert.NotNull(bodyAfterChangeState);
        Assert.False(bodyAfterChangeState.Enabled);
        // Delete the created bodies
        await DeleteBodies([name]);
    }

    private async Task DeleteBodies(string[] names)
    {
        foreach (string name in names)
        {
            using HttpResponseMessage responseDelete = await fixture.Client.DeleteAsync($"/body/{name}");
            Assert.Equal(HttpStatusCode.OK, responseDelete.StatusCode);
        }
        using HttpResponseMessage responseGet = await fixture.Client.GetAsync("/body");
        List<BodyDto>? bodies = await responseGet.Content.ReadFromJsonAsync<List<BodyDto>>();
        Assert.NotNull(bodies);
        Assert.DoesNotContain(bodies, body => names.Contains(body!.Name));
    }
}
