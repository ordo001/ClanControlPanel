namespace ClanControlPanel.Infrastructure.Data;

public class EventDb
{
    public Guid Id { get; set; }
    public DateTime Date  { get; set; }
    public Guid EventTypeId { get; set; }
    public EventTypeDb EventType { get; set; } 
    public int? Status { get; set; }
    // 0,1,2,3 - выигранных этапов потасовки из трех
    // null - gold drop
    
    public List<EventAttendanceDb> Attendences { get; set; }
}