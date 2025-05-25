using Microsoft.AspNetCore.SignalR;

namespace ClanControlPanel.Api.Hubs;

public class PlayerHub : Hub
{
    public async Task NotifyUsersUpdated()
    {
        await Clients.All.SendAsync("PlayersUpdated");
    }
}