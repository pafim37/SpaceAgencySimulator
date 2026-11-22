using Sas.Body.Core.DataTransferObject;
using System.Net;
using System.Net.Http.Json;
using Xunit;
namespace Sas.Integration.Tests;

[Collection("Docker")]
public sealed class BodyIntegrationTest(DockerFixture fixture) : BodyDatabaseTest(fixture)
{
    [Fact]
    public async Task GetBody_ReturnsSeededBodies()
    {
        // Act
        using HttpResponseMessage response = await Client.GetAsync("/body");
        List<BodyDto>? bodySystem = await response.Content.ReadFromJsonAsync<List<BodyDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(bodySystem);
        Assert.Contains("Sun", bodySystem.Select(body => body.Name));
    }


    [Fact]
    public async Task GetBodiesNames_ReturnsBodiesNames()
    {
        // Arrange
        string[] names = ["Earth", "Moon"];
        using HttpResponseMessage responseDefaults = await Client.PostAsJsonAsync("/body/defaults", names);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

        // Act
        using HttpResponseMessage response = await Client.GetAsync("/body/names");
        List<string>? bodyNames = await response.Content.ReadFromJsonAsync<List<string>>();

        // Assert
        Assert.NotNull(bodyNames);
        Assert.Contains("Sun", bodyNames);
        Assert.Contains("Earth", bodyNames);
        Assert.Contains("Moon", bodyNames);
    }

    [Fact]
    public async Task GetBodiesName_ReturnsBody()
    {
        // Arrange
        string[] names = ["Earth", "Moon"];
        using HttpResponseMessage responseDefaults = await Client.PostAsJsonAsync("/body/defaults", names);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

        // Act
        using HttpResponseMessage response = await Client.GetAsync("/body/Earth");
        BodyDto? body = await response.Content.ReadFromJsonAsync<BodyDto>();

        // Assert
        Assert.NotNull(body);
        Assert.Equal("Earth", body.Name);
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
        using HttpResponseMessage responseDefaults = await Client.PostAsJsonAsync("/body", newBody);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);

        // Act
        using HttpResponseMessage response = await Client.GetAsync("/body/Planet");
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
        using HttpResponseMessage responseDefaults = await Client.PostAsJsonAsync("/body", newBody);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);
        // Act
        using HttpResponseMessage responseClone = await Client.PostAsJsonAsync($"/body/clone/{name}", new object());
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
        using HttpResponseMessage responseDefaults = await Client.PostAsJsonAsync("/body", newBody);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);
        BodyDto createdBody = await responseDefaults.Content.ReadFromJsonAsync<BodyDto>()
            ?? throw new InvalidOperationException("The created body response did not contain a body.");
        // Act
        BodyPatchDto patch = new()
        {
            Id = createdBody.Id,
            Name = name,
            Mass = 2.0,
            Radius = 2.0,
            Enabled = false
        };
        using HttpResponseMessage responsePatch = await Client.SendAsync(new HttpRequestMessage(HttpMethod.Patch, "/body")
        {
            Content = JsonContent.Create(patch)
        });
        Assert.Equal(HttpStatusCode.OK, responsePatch.StatusCode);
        using HttpResponseMessage responseGetBody = await Client.GetAsync($"/body/{name}");
        BodyDto? bodyAfterPatch = await responseGetBody.Content.ReadFromJsonAsync<BodyDto>();
        // Assert
        Assert.NotNull(bodyAfterPatch);
        Assert.Equal(2.0, bodyAfterPatch.Mass);
        Assert.Equal(2.0, bodyAfterPatch.Radius);
        Assert.False(bodyAfterPatch.Enabled);
    }

    [Fact]
    public async Task GetSupportedNames_ReturnsNames()
    {
        // Act
        using HttpResponseMessage response = await Client.GetAsync("/body/supported-names");
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
        using HttpResponseMessage responseDefaults = await Client.PostAsJsonAsync("/body", newBody);
        Assert.Equal(HttpStatusCode.Created, responseDefaults.StatusCode);
        // Act
        using HttpResponseMessage responseChangeState = await Client.PostAsJsonAsync($"/body/{name}", false);
        Assert.Equal(HttpStatusCode.OK, responseChangeState.StatusCode);
        using HttpResponseMessage responseGetBody = await Client.GetAsync($"/body/{name}");
        BodyDto? bodyAfterChangeState = await responseGetBody.Content.ReadFromJsonAsync<BodyDto>();
        // Assert
        Assert.NotNull(bodyAfterChangeState);
        Assert.False(bodyAfterChangeState.Enabled);
    }
}
