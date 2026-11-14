using Microsoft.Extensions.Hosting;
using Sas.Body.Infrastructure.Notifications;

namespace Sas.Body.Service;

internal sealed class BodyNotificationHostedService(NotificationClient client) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        client.StartConnection();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => client.StopConnectionAsync();
}
