using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Interfaces.Services;

public interface IEventService
{
    public Task<List<Event>> GetEvents();
    public Task<Event> GetEventById(Guid eventId);
    public Task AddEvent(DateTime date, Guid eventTypeId, int? status); //TODO: хуй знает что лучше eventTypeId или передать string название этапа. Проверить с клиентом
    public Task RemoveEvent(Guid eventId);
    public Task AddEventAttendanceForOnePlayer(Guid eventId, Guid playerId, bool wasPresent, bool? isExcused, string? reason);
    public Task MarkPlayerInEvent(Guid eventId, Guid playerId);
    public Task MarkListPlayersInEvent(Guid eventId, List<string> playerNameList);
    public Task RemoveMarkPlayerFromEvent(Guid eventId, Guid playerId);

}