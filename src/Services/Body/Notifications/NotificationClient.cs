using Microsoft.AspNetCore.SignalR.Client;

namespace Sas.Body.Service.Notifications
{
    public sealed class NotificationClient : IAsyncDisposable
    {
        private static readonly string url = "http://localhost:6443/notification";
        private readonly CancellationTokenSource cancellationTokenSource = new();

        private readonly HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

        public void StartConnection()
        {
            if (connection.State == HubConnectionState.Disconnected)
            {
                TryStart();
            }
        }

        public void TryStart()
        {
            Task.Run(async () =>
            {
                while (connection.State == HubConnectionState.Disconnected && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        await connection.StartAsync();
                        Console.WriteLine($"Connection success!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Connection failed: {ex.Message}. Retrying in 5 seconds...");
                    }

                    await Task.Delay(5000);
                }
            }, cancellationTokenSource.Token).ConfigureAwait(false);
        }

        public async Task StopConnectionAsync()
        {
            if (connection.State == HubConnectionState.Connected)
            {
                await connection.StopAsync();
            }
        }

        public async Task SendBodyDatabaseChangedNotification(string? senderId, CancellationToken cancellationToken)
        {
            if (connection.State == HubConnectionState.Connected)
            {
                if (senderId is null)
                {
                    await connection.InvokeAsync("SendBodyDatabaseChangedNotificationToAll", cancellationToken);
                }
                else 
                {
                    await connection.InvokeAsync("SendBodyDatabaseChangedNotificationExcept", senderId, cancellationToken);
                }
            }
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