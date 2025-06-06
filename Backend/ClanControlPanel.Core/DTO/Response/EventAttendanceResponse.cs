using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.DTO.Response;

public class EventAttendanceResponse
{
    public Guid EventId { get; set; }
    public string EventTypeName { get; set; } = null!;
    public DateTime Date { get; set; }
    public List<AttendanceResponse> Attendances { get; set; } = new();
}
