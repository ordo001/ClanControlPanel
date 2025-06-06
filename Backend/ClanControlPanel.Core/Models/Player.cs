using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Models;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid? SquadId { get; set; }
    public Squad? Squad { get; set; } = null!;
    public int? Position { get; set; } 
    
    public List<Schedule> Schedules { get; set; } = new();
    public List<Equipment> Equipments { get; set; } = new();
    public List<EventAttendance> Attendances { get; set; } = new();
}