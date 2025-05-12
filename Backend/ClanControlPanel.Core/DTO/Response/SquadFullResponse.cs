using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.DTO.Response;

public class SquadFullResponse
{
    public Guid Id { get; set; }
    public string NameSquad { get; set; } = string.Empty;
    public List<PlayersResponse> Players { get; set; } = new();
}