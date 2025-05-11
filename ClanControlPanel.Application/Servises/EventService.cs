using System.Runtime.InteropServices.ComTypes;
using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.DTO.Response;
using ClanControlPanel.Core.Models;
using ClanControlPanel.Infrastructure.Data;
using ClanControlPanel.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Core.Interfaces.Services;

public class EventService(ClanControlPanelContext context, IPlayerService playerService) : IEventService
{
    public async Task<List<Event>> GetEvents()
    {
        try
        {
            var events = await context.Event.ToListAsync();
            return events;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Event> GetEventById(Guid eventId)
    {
        var eventEntity = await context.Event.FirstOrDefaultAsync(p => p.Id == eventId);
        if (eventEntity is null)
            throw new Exception("Событие не найдено");

        return eventEntity;
    }

    public async Task AddEvent(DateTime date, Guid eventTypeId, int? status)
    {
        try
        {
            var eventEntity = new Event
            {
                Date = date,
                EventTypeId = eventTypeId,
                Status = status
            };
            await context.Event.AddAsync(eventEntity);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task RemoveEvent(Guid eventId)
    {
        var eventEntity = await context.Event.FirstOrDefaultAsync(p => p.Id == eventId);
        if (eventEntity is null)
            throw new EntityNotFoundException<Event>(eventId);
        try
        {
            context.Event.Remove(eventEntity);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task AddEventAttendanceForOnePlayer(Guid eventId, Guid playerId, bool wasPresent,
        bool? isExcused, string? reason)
    {
        var eventEntity = await context.Event.FirstOrDefaultAsync(p => p.Id == eventId);
        if (eventEntity is null)
            throw new EntityNotFoundException<Event>(eventId);
        
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
        if (player is null)
            throw new EntityNotFoundException<Player>(playerId);

        try
        {
            var eventAttendance = new EventAttendance
            {
                EventId = eventId,
                PlayerId = playerId,
                WasPresent = wasPresent
            };

            switch (isExcused)
            {
                case true:
                    eventAttendance.IsExcused = true;
                    break;
                case false:
                    eventAttendance.IsExcused = false;
                    eventAttendance.AbsenceReason = reason;
                    break;
            }

            await context.EventAttendences.AddAsync(eventAttendance);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task MarkPlayerInEvent(Guid eventId, Guid playerId)
    {
        var eventEntity = await context.Event.SingleOrDefaultAsync(p => p.Id == eventId);
        if (eventEntity is null)
            throw new EntityNotFoundException<Event>(eventId);
        
        var player = await context.Players.SingleOrDefaultAsync(p => p.Id == playerId);
        if (player is null)
            throw new EntityNotFoundException<Player>(playerId);

        try
        {
            var attendance = await context.EventAttendences
                .SingleOrDefaultAsync(a => a.EventId == eventId && a.PlayerId == playerId);

            if (attendance != null)
            {
                attendance.WasPresent = true;
                attendance.IsExcused = null;
                attendance.AbsenceReason = null;
            }
            else
            {
                attendance = new EventAttendance
                {
                    EventId = eventId,
                    PlayerId = playerId,
                    IsExcused = null,
                    WasPresent = true
                };
                await context.EventAttendences.AddAsync(attendance);
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task MarkListPlayersInEvent(Guid eventId, List<string> playerNameList)
    {
        var eventEntity = await context.Event.SingleOrDefaultAsync(p => p.Id == eventId);
        if (eventEntity is null)
            throw new EntityNotFoundException<Event>(eventId);

        try
        {
            var listMarkPlayers = new List<EventAttendance>();
            foreach (var playerName in playerNameList)
            {
                var player = await playerService.GetPlayerByName(playerName); // Вылетит исклбчение, если ник некорректный и ни один игрок не будет добавлен в ДБ (чтобы не было пропусков)
                
                var attendance = await context.EventAttendences
                    .SingleOrDefaultAsync(a => a.EventId == eventId && a.PlayerId == player.Id);
                if (attendance is not null) // Если пользователь был отмечен как неприсутствующий, то отмечаем как присутсвующего
                {
                    attendance.WasPresent = true;
                    attendance.IsExcused = null;
                    attendance.AbsenceReason = null;
                    continue;
                }
                
                listMarkPlayers.Add(new EventAttendance{EventId = eventId, PlayerId = player.Id}); // Добавляем список всех присутствующих игроков
            }
            await context.EventAttendences.AddRangeAsync(listMarkPlayers); // Добавляем заведомо существующих игроков в бд
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task RemoveMarkPlayerFromEvent(Guid eventId, Guid playerId)
    {
        var eventAttendance = await context.EventAttendences.FirstOrDefaultAsync(p => p.EventId == eventId && p.PlayerId == playerId);
        if (eventAttendance is null)
            throw new EntityNotFoundException<EventAttendance>(eventId);
        try
        {
            context.EventAttendences.Remove(eventAttendance);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<AttendanceDto>> GetPlayerAttendance(Guid playerId)
    {
        try
        {
            var player = await playerService.GetPlayerById(playerId);
            
            var attendancesDto = await context.EventAttendences
                .Include(e => e.Event)
                .ThenInclude(e => e.EventType)
                .Include(e => e.Player)
                .Where(e => e.WasPresent)
                .Select(e => new AttendanceDto
                {
                    Id = e.Id,
                    EventId = e.EventId,
                    EventName = e.Event.EventType.NameEventType,
                    PlayerId = e.PlayerId,
                    PlayerName = e.Player.Name,
                    WasPresent = e.WasPresent,
                    IsExcused = e.IsExcused,
                    AbsenceReason = e.AbsenceReason
                })
                .ToListAsync();

            return attendancesDto;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<AttendanceDto>> GetEventAttendance(Guid eventId)
    {
        try
        {
            var eventEntity = await GetEventById(eventId);
            
            var attendancesDto = await context.EventAttendences
                .Include(e => e.Event)
                .ThenInclude(e => e.EventType)
                .Include(e => e.Player)
                .Where(e => e.WasPresent)
                .Select(e => new AttendanceDto
                {
                    Id = e.Id,
                    EventId = e.EventId,
                    EventName = e.Event.EventType.NameEventType,
                    PlayerId = e.PlayerId,
                    PlayerName = e.Player.Name,
                    WasPresent = e.WasPresent,
                    IsExcused = e.IsExcused,
                    AbsenceReason = e.AbsenceReason
                })
                .ToListAsync();

            return attendancesDto;
        }
        catch (Exception ex)
        {
            throw;
        }
    } 
}