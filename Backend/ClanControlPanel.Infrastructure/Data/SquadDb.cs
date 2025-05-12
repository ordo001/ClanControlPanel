namespace ClanControlPanel.Infrastructure.Data;

public class SquadDb
{
    public Guid Id { get; set; }
    public string NameSquad { get; set; }
    
    public List<PlayerDb> Players { get; set; } = new();
}