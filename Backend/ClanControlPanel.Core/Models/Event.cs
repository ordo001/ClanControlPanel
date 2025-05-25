namespace ClanControlPanel.Core.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Date  { get; set; }
    public Guid EventTypeId { get; set; }
    public EventType EventType { get; set; } 
    /// <summary>
    /// Для турнира/потасовки: сколько этапов выиграно (0–3 или 0–4). 
    /// Для gold drop — null.
    /// </summary>
    public int? Status { get; set; }
    /// <summary>
    /// Список всех «движений казны» по этапам
    /// </summary>
    public ICollection<EventStage> Stages { get; set; }
    /// <summary>
    /// Посещаемость игроков
    /// </summary>
    
    public List<EventAttendance> Attendances { get; set; }
}