using Microsoft.AspNetCore.SignalR;

namespace ClanControlPanel.Api.Hubs;

public class PlayerHub : Hub
{
    public async Task NotifyPlayersUpdated()
    {
        await Clients.All.SendAsync("PlayersUpdated");
    }
}