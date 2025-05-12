namespace ClanControlPanel.Core.DTO;

public class PlayersResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
}