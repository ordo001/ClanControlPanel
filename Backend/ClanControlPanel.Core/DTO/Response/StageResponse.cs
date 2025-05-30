namespace ClanControlPanel.Core.DTO.Response;

public class StageResponse
{
    public Guid IdEventStage { get; set; }
    public int? StageNumber { get; set; }
    public string? Description { get; set; }
    public int Amount { get; set; }
}