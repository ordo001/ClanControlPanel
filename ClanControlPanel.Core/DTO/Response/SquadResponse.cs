namespace ClanControlPanel.Core.DTO.Response;

public class SquadResponse
{
    public Guid IdPlayer { get; set; }
    public string NamePlayer { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid? IdSquad { get; set; } 
}