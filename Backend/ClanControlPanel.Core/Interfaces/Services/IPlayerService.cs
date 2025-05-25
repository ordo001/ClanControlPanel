using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Interfaces.Services;

public interface IPlayerService
{
    public Task<List<Player>> GetPlayers();
    public Task AddPlayer(string name, Guid? squadId);
    public Task<Player> GetPlayerById(Guid id);
    public Task<Player> GetPlayerByName(string name);
    public Task RemovePlayerById(Guid id);
    public Task AddPlayerInSquad(Guid playerId, Guid squadId, int position);
    public Task RemovePlayerFromSquad(Guid playerId);
    
}