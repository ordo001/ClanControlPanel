using ClanControlPanel.Core.Models;
using ClanControlPanel.Infrastructure.Data;

namespace ClanControlPanel.Infrastructure.Mappings;

public static class EntityMappers
{
    public static Player ToDomain(this PlayerDb db)
    {
        return new Player
        {
            Id = db.Id,
            Name = db.Name,
            UserId = db.UserId,
            User = db.User.ToDomain(),
            SquadId = db.SquadId,
            Squad = db.Squad.ToDomain(),
            Schedules = db.Schedules?.Select(s => s.ToDomain()).ToList() ?? new List<Schedule>(),
            Equipments = db.Equipments?.Select(e => e.ToDomain()).ToList() ?? new List<Equipment>(),
            Attendences = db.Attendences?.Select(a => a.ToDomain()).ToList() ?? new List<EventAttendance>()
        };
    }

    public static PlayerDb ToDb(this Player domain)
    {
        return new PlayerDb
        {
            Id = domain.Id,
            Name = domain.Name,
            User = domain.User.ToDb(),
            UserId = domain.UserId,
            SquadId = domain.SquadId,
            Squad = domain.Squad.ToDb(),
            Schedules = domain.Schedules?.Select(s => s.ToDb()).ToList() ?? new List<ScheduleDb>(),
            Equipments = domain.Equipments?.Select(e => e.ToDb()).ToList() ?? new List<EquipmentDb>(),
            Attendences = domain.Attendences?.Select(a => a.ToDb()).ToList() ?? new List<EventAttendanceDb>()
        };
    }
    
    public static User ToDomain(this UserDb db) =>
        new User
        {
            Id = db.Id,
            Login = db.Login,
            PasswordHash = db.PasswordHash,
            Name = db.Name,
            Role = db.Role
        };

    public static UserDb ToDb(this User domain) =>
        new UserDb
        {
            Id = domain.Id,
            Login = domain.Login,
            PasswordHash = domain.PasswordHash,
            Name = domain.Name,
            Role = domain.Role
        };

    public static Squad ToDomain(this SquadDb db) =>
        new Squad
        {
            Id = db.Id,
            NameSquad = db.NameSquad,
            Players = db.Players?.Select(p => p.ToDomain()).ToList() ?? new List<Player>()
        };

    public static SquadDb ToDb(this Squad domain) =>
        new SquadDb
        {
            Id = domain.Id,
            NameSquad = domain.NameSquad,
            Players = domain.Players?.Select(p => p.ToDb()).ToList() ?? new List<PlayerDb>()
        };

    public static Schedule ToDomain(this ScheduleDb db) =>
        new Schedule
        {
            Id = db.Id,
            PlayerId = db.PlayerId,
            DayOfWeek = db.DayOfWeek
        };

    public static ScheduleDb ToDb(this Schedule domain) =>
        new ScheduleDb
        {
            Id = domain.Id,
            PlayerId = domain.PlayerId,
            DayOfWeek = domain.DayOfWeek
        };

    public static Item ToDomain(this ItemDb db) =>
        new Item
        {
            Id = db.Id,
            Name = db.Name,
            Description = db.Description
        };

    public static ItemDb ToDb(this Item domain) =>
        new ItemDb
        {
            Id = domain.Id,
            Name = domain.Name,
            Description = domain.Description
        };

    public static EventType ToDomain(this EventTypeDb db) =>
        new EventType
        {
            Id = db.Id,
            NameEventType = db.NameEventType
        };

    public static EventTypeDb ToDb(this EventType domain) =>
        new EventTypeDb
        {
            Id = domain.Id,
            NameEventType = domain.NameEventType
        };

    public static Event ToDomain(this EventDb db) =>
        new Event
        {
            Id = db.Id,
            Date = db.Date,
            EventTypeId = db.EventTypeId,
            Status = db.Status,
            Attendences = db.Attendences?.Select(a => a.ToDomain()).ToList() ?? new List<EventAttendance>()
        };

    public static EventDb ToDb(this Event domain) =>
        new EventDb
        {
            Id = domain.Id,
            Date = domain.Date,
            EventTypeId = domain.EventTypeId,
            Status = domain.Status,
            Attendences = domain.Attendences?.Select(a => a.ToDb()).ToList() ?? new List<EventAttendanceDb>()
        };

    public static EventAttendance ToDomain(this EventAttendanceDb db) =>
        new EventAttendance
        {
            Id = db.Id,
            EventId = db.EventId,
            PlayerId = db.PlayerId,
            WasPresent = db.WasPresent,
            IsExcused = db.IsExcused,
            AbsenceReason = db.AbsenceReason
        };

    public static EventAttendanceDb ToDb(this EventAttendance domain) =>
        new EventAttendanceDb
        {
            Id = domain.Id,
            EventId = domain.EventId,
            PlayerId = domain.PlayerId,
            WasPresent = domain.WasPresent,
            IsExcused = domain.IsExcused,
            AbsenceReason = domain.AbsenceReason
        };

    public static Equipment ToDomain(this EquipmentDb db) =>
        new Equipment
        {
            Id = db.Id,
            PlayerId = db.PlayerId,
            ItemId = db.ItemId
        };

    public static EquipmentDb ToDb(this Equipment domain) =>
        new EquipmentDb
        {
            Id = domain.Id,
            PlayerId = domain.PlayerId,
            ItemId = domain.ItemId
        };

    public static ClanMoney ToDomain(this ClanMoneyDb db) =>
        new ClanMoney
        {
            Id = db.Id,
            ActionDate = db.ActionDate,
            TotalAmountAfterAction = db.TotalAmountAfterAction,
            ChangeAmount = db.ChangeAmount,
            Reason = db.Reason,
            CustomReason = db.CustomReason
        };

    public static ClanMoneyDb ToDb(this ClanMoney domain) =>
        new ClanMoneyDb
        {
            Id = domain.Id,
            ActionDate = domain.ActionDate,
            TotalAmountAfterAction = domain.TotalAmountAfterAction,
            ChangeAmount = domain.ChangeAmount,
            Reason = domain.Reason,
            CustomReason = domain.CustomReason
        };
}