using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.DTO;

public class AttendanceUpdateRequest
{
    public AttendanceStatus Status { get; set; }
    public string? AbsenceReason { get; set; }
}