namespace ClanControlPanel.Core.Models;

public class EventType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NameEventType { get; set; }
}