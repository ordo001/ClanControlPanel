namespace ClanControlPanel.Core.DTO;

public class EventStageAddRequest
{
    public int? StageNumber { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
}