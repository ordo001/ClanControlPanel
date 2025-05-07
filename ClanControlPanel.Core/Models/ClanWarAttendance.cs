namespace ClanControlPanel.Core.Models;

public class ClanWarAttendance
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public bool WasPresent { get; set; } = false;
    public string? AbsenceReason { get; set; } = string.Empty;
}