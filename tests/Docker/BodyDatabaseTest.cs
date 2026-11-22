using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Sas.Integration.Tests;

public abstract class BodyDatabaseTest(DockerFixture fixture) : IAsyncLifetime
{
    protected HttpClient Client => fixture.Client;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        // Each Compose fixture seeds its own database with the Sun.
        string[]? names = await Client.GetFromJsonAsync<string[]>("/body/names");
        Assert.NotNull(names);
        foreach (string name in names.Where(name => name != "Sun"))
        {
            using HttpResponseMessage response = await Client.DeleteAsync($"/body/{Uri.EscapeDataString(name)}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
