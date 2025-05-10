using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Interfaces.Services;

public interface ISquadService
{
    public Task UpdateNameSquad(Guid squadId, string name);
    public Task<Squad> CreateSquad(string name);
    public Task<List<Player>> GetPlayersInSquad(Guid squadId);
    public Task<List<Squad>> GetSquads();
}