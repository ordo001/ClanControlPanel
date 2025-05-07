namespace ClanControlPanel.Core.Models;

public class Player
{
    public Guid IdUser { get; set; }
    public string Name { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public List<Equipment> Equipments { get; set; } = new();
}