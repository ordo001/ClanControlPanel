namespace ClanControlPanel.Core.Models;

public class GoldenDropAttendance
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    
}