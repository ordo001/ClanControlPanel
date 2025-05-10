namespace ClanControlPanel.Core.DTO;

public class PlayerAddRequest
{
    public string Name { get; set; } = String.Empty;
    public Guid UserId { get; set; }
}