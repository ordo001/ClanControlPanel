namespace ClanControlPanel.Core.Models;

public class Squad
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NameSquad { get; set; }
    
    public List<Player> Players { get; set; } = new();
}