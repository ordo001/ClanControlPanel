namespace ClanControlPanel.Core.Models;

public enum AttendanceStatus
{
    Present            = 0,   // игрок пришёл
    AbsentUnexcused    = 1,   // отсутствовал, не сообщил
    AbsentExcused      = 2    // отсутствовал, сообщил заранее или по графику
}