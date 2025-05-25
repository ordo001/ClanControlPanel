using Microsoft.AspNetCore.SignalR;

namespace ClanControlPanel.Api.Hubs;

public class UserHub : Hub
{
    public async Task NotifyUsersUpdated()
    {
        await Clients.All.SendAsync("UsersUpdated");
    }
}