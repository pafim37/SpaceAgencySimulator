using Microsoft.AspNetCore.SignalR;

namespace Sas.Notification.Service.Hubs
{
    public class MasterHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task SendBodyDatabaseChangedNotification()
        {
            await Clients.All.SendAsync("BodyDatabaseChanged");
        }
    }
}
