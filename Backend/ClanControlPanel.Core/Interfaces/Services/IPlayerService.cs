using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Interfaces.Services;

public interface IPlayerService
{
    public Task<List<Player>> GetPlayers();
    public Task AddPlayer(Guid userId, string name);
    public Task<Player> GetPlayerById(Guid id);
    public Task<Player> GetPlayerByName(string name);
    public Task RemovePlayerById(Guid id);
    public Task AddPlayerInSquad(Guid playerId, Guid squadId);
    public Task RemovePlayerFromSquad(Guid playerId);
    
}