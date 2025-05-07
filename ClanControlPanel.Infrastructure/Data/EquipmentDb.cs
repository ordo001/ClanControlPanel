namespace ClanControlPanel.Infrastructure.Data;

public class EquipmentDb
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }
    public PlayerDb PlayerDb { get; set; } = null!;
    public Guid ItemDbId { get; set; }
    public ItemDb ItemDb { get; set; } = null!;
}