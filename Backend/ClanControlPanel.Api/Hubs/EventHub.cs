using Microsoft.AspNetCore.SignalR;

namespace ClanControlPanel.Api.Hubs;

public class EventHub : Hub
{
    public async Task NotifyEventsUpdated()
    {
        await Clients.All.SendAsync("EventUpdated");
    }
    
    public async Task NotifyAttendancesUpdated()
    {
        await Clients.All.SendAsync("AttendanceUpdated");
    }
}