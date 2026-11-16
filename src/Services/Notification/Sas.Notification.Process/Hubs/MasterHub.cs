using Microsoft.AspNetCore.SignalR;

namespace Sas.Notification.Service.Hubs
{
    public class MasterHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task SendBodyDatabaseChangedNotificationToAll()
        {
            await Clients.All.SendAsync("BodyDatabaseChanged");
        }

        public async Task SendBodyDatabaseChangedNotificationExcept(string except)
        {
            await Clients.AllExcept(except).SendAsync("BodyDatabaseChanged");
        }
    }
}
