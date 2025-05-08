namespace ClanControlPanel.Infrastructure.Data;

public class PlayerDb
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserDb User { get; set; } = null!;
    public Guid? SquadId { get; set; }
    public SquadDb Squad { get; set; } = null!;
    
    public List<ScheduleDb> Schedules { get; set; } = new();
    public List<EquipmentDb> Equipments { get; set; } = new();
    public List<EventAttendenceDb> Attendences { get; set; } = new();
}