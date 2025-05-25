namespace ClanControlPanel.Core.Models;

public class EventAttendance
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public Event Event { get; set; }
    public Guid PlayerId { get; set; }
    public Player Player { get; set; }
    
    /*public bool WasPresent { get; set; }*/
    
    public AttendanceStatus Status { get; set; }
    public string? AbsenceReason { get; set; }
}