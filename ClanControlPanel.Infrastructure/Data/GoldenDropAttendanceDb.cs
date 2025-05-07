namespace ClanControlPanel.Infrastructure.Data;

public class GoldenDropAttendanceDb
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public Guid PlayerId { get; set; }
    public PlayerDb PlayerDb { get; set; } = null!;
    
}