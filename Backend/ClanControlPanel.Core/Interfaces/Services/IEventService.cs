using ClanControlPanel.Core.DTO.Response;
using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Interfaces.Services;

public interface IEventService
{
    public Task<List<EventResponse>> GetEvents();
    public Task<Event> GetEventById(Guid eventId);
    public Task AddEvent(DateTime date, Guid eventTypeId, int? wonStagesCount = null); //TODO: хуй знает что лучше eventTypeId или передать string название этапа. Проверить с клиентом
    public Task RemoveEvent(Guid eventId);
    public Task<List<AttendanceDto>> GetPlayerAttendance(Guid playerId);
    public Task<List<AttendanceDto>> GetEventAttendance(Guid eventId);
    public Task SetAttendance(Guid eventId, Guid playerId, AttendanceStatus status, string? absenceReason = null);
    public Task MarkPlayersPresent(Guid eventId, IEnumerable<Guid> playerIds);
    public Task RemoveAttendance(Guid eventId, Guid playerId);

    
    // Казна
    Task<List<EventStage>> GetEventStages(Guid eventId);
    Task AddEventStage(Guid eventId, int stageNumber, int amount, string? description = null);
    Task UpdateEventStage(Guid stageId, int amount, string? description = null);
    Task RemoveEventStage(Guid stageId);
}