namespace ClanControlPanel.Infrastructure.Data;

public class EventAttendenceDb
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public EventDb Event { get; set; }
    public Guid PlayerId { get; set; }
    public PlayerDb Player { get; set; }
    
    public bool WasPresent { get; set; }
    
    public bool? IsExcused { get; set; } = false;
    // null = пришёл
    // false = отсутствовал, не сообщил
    // true = отсутствовал, сообщил или по графику
    public string? AbsenceReason { get; set; }
}