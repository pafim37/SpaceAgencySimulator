using Microsoft.AspNetCore.SignalR.Client;

namespace Sas.Body.Service.Notifications
{
    public sealed class NotificationService(string url) : IAsyncDisposable
    {
        private readonly HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

        public async Task StartConnectionAsync()
        {
            if (connection.State == HubConnectionState.Disconnected)
            {
                await connection.StartAsync();
            }
        }

        public async Task StopConnectionAsync()
        {
            if (connection.State == HubConnectionState.Connected)
            {
                await connection.StartAsync();
            }
        }

        public async Task SendBodyDatabaseChangedNotification(CancellationToken cancellationToken)
        {
            await connection.InvokeAsync("SendBodyDatabaseChangedNotification", cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (connection.State == HubConnectionState.Connected)
            {
                await connection.StopAsync();
            }
        }
    }
}