using System.Diagnostics;
using Xunit;

namespace Sas.Integration.Tests;

[CollectionDefinition("Docker")]
public sealed class DockerCollection : ICollectionFixture<DockerFixture>
{
}

public sealed class DockerFixture : IAsyncLifetime
{
    private readonly string repositoryRoot = FindRepositoryRoot();
    private readonly string projectName = $"sas-integ-tests-{Guid.NewGuid():N}";
    private readonly bool isDebugging = Debugger.IsAttached;

    public HttpClient Client { get; } = new() { Timeout = TimeSpan.FromSeconds(5) };

    public async Task InitializeAsync()
    {
        if (isDebugging)
        {
            Client.Timeout = Timeout.InfiniteTimeSpan;
        }

        try
        {
            await RunComposeAsync("up", "--build", "--detach");
            string address = await RunComposeAsync("port", "body-system", "3742");
            Client.BaseAddress = new Uri($"http://{address.Trim()}/");
            await WaitForBodySystemAsync();
        }
        catch (Exception exception)
        {
            string logs;
            try
            {
                logs = await RunComposeAsync("logs", "--no-color");
            }
            catch (Exception logException)
            {
                logs = logException.Message;
            }

            throw new InvalidOperationException($"Docker fixture failed to start.{Environment.NewLine}{logs}", exception);
        }
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        await RunComposeAsync("down", "--volumes", "--rmi", "local");
    }

    private async Task WaitForBodySystemAsync()
    {
        Stopwatch elapsed = Stopwatch.StartNew();
        TimeSpan startupTimeout = TimeSpan.FromSeconds(90);
        while (true)
        {
            try
            {
                using CancellationTokenSource requestTimeout = new(TimeSpan.FromSeconds(5));
                using HttpResponseMessage response = await Client.GetAsync("/body", requestTimeout.Token);
                response.EnsureSuccessStatusCode();
                return;
            }
            catch (Exception exception) when (
                (exception is HttpRequestException or TaskCanceledException) && elapsed.Elapsed < startupTimeout)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }

    private async Task<string> RunComposeAsync(params string[] arguments)
    {
        ProcessStartInfo startInfo = new("docker")
        {
            WorkingDirectory = repositoryRoot,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        startInfo.Environment["SAS_BODYSYSTEM_PORT"] = "0";
        startInfo.Environment["SAS_NOTIFICATION_PORT"] = "0";
        if (isDebugging)
        {
            startInfo.Environment["SAS_BODYSYSTEM_BUILD_CONFIGURATION"] = "Debug";
        }

        string composeFile = Path.Combine(repositoryRoot, ".docker", "docker-compose.yaml");
        foreach (string argument in new[] { "compose", "--file", composeFile, "--project-name", projectName }.Concat(arguments))
        {
            startInfo.ArgumentList.Add(argument);
        }

        using Process process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Unable to start Docker.");
        Task<string> stdout = process.StandardOutput.ReadToEndAsync();
        Task<string> stderr = process.StandardError.ReadToEndAsync();
        using CancellationTokenSource timeout = new(TimeSpan.FromMinutes(10));

        try
        {
            await process.WaitForExitAsync(timeout.Token);
        }
        catch (OperationCanceledException) when (timeout.IsCancellationRequested)
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException($"Timed out running docker compose {string.Join(' ', arguments)}.");
        }

        string standardOutput = await stdout;
        string standardError = await stderr;

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"docker compose {string.Join(' ', arguments)} exited with code {process.ExitCode}.{Environment.NewLine}{standardOutput}{Environment.NewLine}{standardError}");
        }

        return standardOutput;
    }

    private static string FindRepositoryRoot()
    {
        for (DirectoryInfo? directory = new(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, ".docker", "docker-compose.yaml")))
            {
                return directory.FullName;
            }
        }

        throw new DirectoryNotFoundException("Could not locate the repository's .docker/docker-compose.yaml.");
    }
}
