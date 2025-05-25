namespace ClanControlPanel.Core.DTO;

public class PlayersResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public int? Position { get; set; }
    /*public Guid? SquadId { get; set; }
    public string? SquadName { get; set; } = string.Empty;*/
}