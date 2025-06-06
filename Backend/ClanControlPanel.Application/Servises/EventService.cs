using System.Runtime.InteropServices.ComTypes;
using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.DTO.Response;
using ClanControlPanel.Core.Models;
using ClanControlPanel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Core.Interfaces.Services;

public class EventService(ClanControlPanelContext context, IPlayerService playerService) : IEventService
{
    public async Task<List<EventResponse>> GetEvents()
    {
        var events = await context.Events
            .AsNoTracking()
            .Include(e => e.Stages)
            .Include(e => e.EventType)
            .ToListAsync();
        return events.Select(e => new EventResponse
            {
                IdEvent = e.Id,
                Date = e.Date,
                Status = e.Status,
                EventTypeName = e.EventType.NameEventType,
                Stages = e.Stages.Select(s => new StageResponse
                {
                    IdEventStage = s.Id,
                    StageNumber = s.StageNumber,
                    Amount = s.Amount,
                    Description = s.Description
                }).ToList()
            })
            .OrderByDescending(e => e.Date)
            .ToList();
    }

    public async Task<List<EventTypesResponse>> GetEventTypes()
    {
        var eventTypes = await context.EventTypes
            .AsNoTracking()
            .ToListAsync();

        return eventTypes.Select(e => new EventTypesResponse
        {
            Id = e.Id,
            EventTypeName = e.NameEventType
        }).ToList();
    }

    public async Task<Event> GetEventById(Guid eventId)
    {
        var eventEntity = await context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == eventId);
        if (eventEntity is null)
            throw new EntityNotFoundException<Event>(eventId);

        return eventEntity;
    }

    public async Task<Guid> AddEvent(DateTime date, Guid eventTypeId, int? status)
    {
        var eventEntity = new Event
        {
            Date = date,
            EventTypeId = eventTypeId,
            Status = status
        };
        await context.Events.AddAsync(eventEntity);

        var players = await context.Players.ToListAsync();

        await context.EventAttendances.AddRangeAsync(players.Select(p => new EventAttendance
        {
            EventId = eventEntity.Id,
            PlayerId = p.Id,
            Status = AttendanceStatus.AbsentUnexcused,
            AbsenceReason = null
        }));

        await context.SaveChangesAsync();
        return eventEntity.Id;
    }

    public async Task RemoveEvent(Guid eventId)
    {
        var eventEntity = await context.Events.FirstOrDefaultAsync(p => p.Id == eventId);
        if (eventEntity is null)
            throw new EntityNotFoundException<Event>(eventId);
        context.Events.Remove(eventEntity);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Отметить одного игрока в событии с конкретным статусом посещаемости.
    /// </summary>
    public async Task SetAttendance(
        Guid eventId,
        Guid playerId,
        AttendanceStatus status,
        string? absenceReason = null)
    {
        var ev = await context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId);
        if (ev == null)
            throw new EntityNotFoundException<Event>(eventId);

        var player = await context.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == playerId);
        if (player is null)
            throw new EntityNotFoundException<Player>(playerId);

        var attendance = await context.EventAttendances
            .SingleOrDefaultAsync(a => a.EventId == eventId && a.PlayerId == playerId);

        var aboba = await context.EventAttendances
            .Where(a => a.EventId == eventId && a.PlayerId == playerId)
            .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.Status, status)
                .SetProperty(b => b.AbsenceReason, absenceReason));

        /*if (attendance == null)
        {
            attendance = new EventAttendance
            {
                EventId = eventId,
                PlayerId = playerId
            };
            context.EventAttendances.Add(attendance);
        }*/

        if (aboba == 0)
        {
            attendance = new EventAttendance
            {
                EventId = eventId,
                PlayerId = playerId,
                Status = status
            };
            context.EventAttendances.Add(attendance);
        }

        /*attendance.Status = status;
        attendance.AbsenceReason = (status == AttendanceStatus.AbsentUnexcused || status == AttendanceStatus.AbsentExcused)
            ? absenceReason
            : null;*/

        await context.SaveChangesAsync();
    }

    public async Task UpdateAttendance(Guid eventId, Guid playerId, AttendanceStatus status)
    {
        var ev = await context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId);
        if (ev == null)
            throw new EntityNotFoundException<Event>(eventId);

        var player = await context.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == playerId);
        if (player is null)
            throw new EntityNotFoundException<Player>(playerId);

        var attendance = await context.EventAttendances
            .SingleOrDefaultAsync(a => a.EventId == eventId && a.PlayerId == playerId);

        attendance!.Status = status;
        context.EventAttendances.Update(attendance);
        await context.SaveChangesAsync();
    }

    /*public async Task MarkPlayerInEvent(Guid eventId, Guid playerId)
    {
        var eventEntity = await context.Event.SingleOrDefaultAsync(p => p.Id == eventId);
        if (eventEntity is null)
            throw new EntityNotFoundException<Event>(eventId);

        var player = await context.Players.SingleOrDefaultAsync(p => p.Id == playerId);
        if (player is null)
            throw new EntityNotFoundException<Player>(playerId);

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
    }*/

    /// <summary>
    /// Отметить список игроков как присутствующих.
    /// </summary>
    public async Task MarkPlayersPresent(Guid eventId, IEnumerable<Guid> playerIds)
    {
        var ev = await context.Events
            .AsNoTracking()
            .FirstAsync(e => e.Id == eventId);
        if (ev == null)
            throw new EntityNotFoundException<Event>(eventId);

        var attendances = new List<EventAttendance>();

        var playerList = await context.Players.AsNoTracking().ToListAsync();

        foreach (var playerId in playerIds)
        {
            if (playerList.FirstOrDefault(p => p.Id == playerId) is null)
                throw new EntityNotFoundException<Player>(playerId);

            var att = await context.EventAttendances
                .SingleOrDefaultAsync(a => a.EventId == eventId && a.PlayerId == playerId);

            if (att == null)
            {
                attendances.Add(new EventAttendance
                {
                    EventId = eventId,
                    PlayerId = playerId,
                    Status = AttendanceStatus.Present
                });
            }
            else
            {
                att.Status = AttendanceStatus.Present;
                att.AbsenceReason = null;
            }
        }

        if (attendances.Count > 0)
            await context.EventAttendances.AddRangeAsync(attendances);

        await context.SaveChangesAsync();
    }

    public async Task RemoveAttendance(Guid eventId, Guid playerId)
    {
        var att = await context.EventAttendances
            .FirstOrDefaultAsync(a => a.EventId == eventId && a.PlayerId == playerId);

        if (att == null)
            throw new EntityNotFoundException<EventAttendance>($"{eventId}:{playerId}");

        context.EventAttendances.Remove(att);
        await context.SaveChangesAsync();
    }

    public async Task<List<EventAttendanceResponse>> GetPlayerAttendance(Guid playerId)
    {
        // проверяем наличие игрока
        var player = await context.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == playerId);
        if (player is null)
            throw new EntityNotFoundException<Player>(playerId);

        return new List<EventAttendanceResponse>();

        /*return await context.EventAttendances
            .Where(a => a.PlayerId == playerId)
            .Include(a => a.Event).ThenInclude(e => e.EventType)
            .Select(a => new EventAttendanceResponse
            {
                AttendanceId = a.Id,
                Date = a.Event.Date,
                Attendances = new List<AttendanceResponse>
                {
                    new AttendanceResponse()
                    {
                        EventId = a.EventId,
                        EventName = a.Event.EventType.NameEventType,
                        PlayerId = a.PlayerId,
                        PlayerName = a.Player.Name,
                        Attendance = a.Status,
                        AbsenceReason = a.AbsenceReason
                    }
                }
            })
            .ToListAsync();*/
    }


    public async Task<EventAttendanceResponse> GetEventAttendance(Guid eventId)
    {
        var eventEntity = await context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId);
        if (eventEntity is null)
            throw new EntityNotFoundException<Event>(eventId);

        var attendance = await context.EventAttendances
            .AsNoTracking()
            .Include(a => a.Player)
                .ThenInclude(p => p.Squad)
            .Include(a => a.Event)
                .ThenInclude(e => e.EventType)
            .Where(a => a.EventId == eventId)
            .ToListAsync();
        

        var eventAttendanceResponse = new EventAttendanceResponse()
        {
            EventId = attendance[0].EventId,
            EventTypeName = attendance[0].Event.EventType.NameEventType,
            Date = attendance[0].Event.Date,
            Attendances = attendance.Select(a => new AttendanceResponse
            {
                AttendanceId = a.Id,
                PlayerId = a.PlayerId,
                PlayerName = a.Player.Name,
                SquadName = a.Player.Squad!.NameSquad,
                Attendance = a.Status,
                AbsenceReason = a.AbsenceReason
            }).ToList()
        };
        return eventAttendanceResponse;
    }

    public async Task<List<EventStage>> GetEventStages(Guid eventId)
    {
        return await context.EventStages
            .AsNoTracking()
            .Where(s => s.EventId == eventId)
            .OrderBy(s => s.StageNumber)
            .ToListAsync();
    }

    public async Task AddEventStage(Guid eventId, int? stageNumber, int amount, string? description = null)
    {
        var ev = await context.Events
                     .AsNoTracking()
                     .FirstAsync(e => e.Id == eventId)
                 ?? throw new EntityNotFoundException<Event>(eventId);

        var stage = new EventStage
        {
            EventId = eventId,
            StageNumber = stageNumber,
            Amount = amount,
            Description = description
        };

        context.EventStages.Add(stage);
        await context.SaveChangesAsync();
    }

    public async Task UpdateEventStage(Guid stageId, int amount, string? description = null)
    {
        var stage = await context.EventStages.FindAsync(stageId)
                    ?? throw new EntityNotFoundException<EventStage>(stageId);

        stage.Amount = amount;
        stage.Description = description;
        await context.SaveChangesAsync();
    }

    public async Task RemoveEventStage(Guid stageId)
    {
        var stage = await context.EventStages.FindAsync(stageId)
                    ?? throw new EntityNotFoundException<EventStage>(stageId);

        context.EventStages.Remove(stage);
        await context.SaveChangesAsync();
    }
}