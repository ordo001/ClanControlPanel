namespace ClanControlPanel.Infrastructure.Data;

public class ScheduleDb
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }
    public PlayerDb Player { get; set; } = null!;

    public DayOfWeek DayOfWeek { get; set; }
}