namespace ClanControlPanel.Infrastructure.Data;

public class EquipmentDb
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }
    public PlayerDb Player { get; set; } = null!;
    public Guid ItemId { get; set; }
    public ItemDb Item { get; set; } = null!;
}