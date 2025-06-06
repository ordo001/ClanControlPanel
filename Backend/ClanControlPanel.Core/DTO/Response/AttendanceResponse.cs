using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.DTO.Response;

public class AttendanceResponse
{
    public Guid AttendanceId { get; set; }
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = null!;
    public string SquadName { get; set; } = null!;
    public AttendanceStatus Attendance { get; set; }
    public string? AbsenceReason { get; set; }
}