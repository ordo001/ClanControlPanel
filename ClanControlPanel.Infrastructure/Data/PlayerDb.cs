namespace ClanControlPanel.Infrastructure.Data;

public class PlayerDb
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserDb UserDb { get; set; } = null!;
    
    /*public List<EquipmentDb> Equipments { get; set; } = new();*/
    public List<ScheduleDb> Schedules { get; set; } = new();
    public List<EquipmentDb> Equipments { get; set; } = new();
    public List<GoldenDropAttendanceDb> GoldenDropAttendances { get; set; } = new();
    public List<ClanWarAttendanceDb> ClanWarAttendances { get; set; } = new();
}