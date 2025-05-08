namespace ClanControlPanel.Core.Models;

public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Date  { get; set; }
    public Guid EventTypeId { get; set; }
    public EventType EventType { get; set; } 
    public int? Status { get; set; }
    // 0,1,2,3 - выигранных этапов потасовки из трех
    // null - gold drop
    
    public List<EventAttendence> Attendences { get; set; }
}