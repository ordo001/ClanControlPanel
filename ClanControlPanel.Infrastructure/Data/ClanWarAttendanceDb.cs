namespace ClanControlPanel.Infrastructure.Data;

public class ClanWarAttendanceDb
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public Guid PlayerId { get; set; }
    public PlayerDb PlayerDb { get; set; } = null!;

    public bool WasPresent { get; set; } = false;
    public string? AbsenceReason { get; set; } = string.Empty;
}