namespace ClanControlPanel.Core.Models;

public class Schedule
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public DayOfWeek DayOfWeek { get; set; }
}