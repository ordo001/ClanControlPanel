namespace ClanControlPanel.Core.Models;

public class EventStage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public Event Event { get; set; }

    /// <summary>Порядковый номер этапа: 1,2,3 для потасовки; 1..4 для турнира; null для gold drop</summary>
    public int? StageNumber { get; set; }
    
    public string? Description { get; set; }

    /// <summary>Положительное — получили; отрицательное — потратили</summary>
    public int Amount { get; set; }
}