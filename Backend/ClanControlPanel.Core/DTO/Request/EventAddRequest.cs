namespace ClanControlPanel.Core.DTO;

public class EventAddRequest
{
    public DateTime Date { get; set; }
    public Guid EventTypeId { get; set; }
    public int? Status { get; set; }
}