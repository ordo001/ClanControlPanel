namespace ClanControlPanel.Core.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Date  { get; set; }
    public Guid EventTypeId { get; set; }
    public EventType EventType { get; set; } 
    public int? Status { get; set; }
    // 0,1,2,3,4 - выигранных этапов потасовки или турнира из четырёх (трёх)
    // null - gold drop
    
    public List<EventAttendance> Attendences { get; set; }
}