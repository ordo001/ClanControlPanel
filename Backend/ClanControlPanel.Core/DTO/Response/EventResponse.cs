using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.DTO.Response;

public class EventResponse
{
    public Guid IdEvent { get; set; }
    public DateTime Date { get; set; }
    public Guid EventTypeId { get; set; }
    public string EventTypeName { get; set; } = string.Empty;
    public int? Status { get; set; }
    public List<StageResponse> Stages { get; set; } = new List<StageResponse>();
    
}