using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.DTO.Response;

public class AttendanceDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string EventName { get; set; } = null!;
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = null!;

    public AttendanceStatus Attendance { get; set; }
    public string? AbsenceReason { get; set; }
}
