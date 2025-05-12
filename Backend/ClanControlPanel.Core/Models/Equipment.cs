namespace ClanControlPanel.Core.Models;

public class Equipment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public Guid ItemId { get; set; }
    public Item Item { get; set; } = null!;
}