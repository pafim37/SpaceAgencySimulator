using Microsoft.AspNetCore.SignalR;

namespace Sas.BodySystem.Service.Hubs
{
    public class BodySystemHub : Hub
    {
        public Task UpdateBoard()
        {
            return Clients.All.SendAsync("Hello World");
        }
    }
}
