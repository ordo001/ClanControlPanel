using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using ClanControlPanel.Infrastructure.Data;
using ClanControlPanel.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Application.Servises;

public class PlayerService(IUserServices userService, ClanControlPanelContext context) : IPlayerService
{
    public async Task<List<Player>> GetPlayers()
    {
        var players = await context.Players.ToListAsync();
        return players;
    }

    public async Task AddPlayer(Guid userId, string name)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            throw new EntityNotFoundException<User>(userId);
        }

        var player = new Player
        {
            Name = name,
            UserId = userId
        };
        await context.Players.AddAsync(player);
        await context.SaveChangesAsync();
    }

    public async Task<Player> GetPlayerById(Guid id)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == id);
        if (player is null)
        {
            throw new EntityNotFoundException<Player>(id);
        }

        return player;
    }

    public async Task<Player> GetPlayerByName(string name)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Name == name);
        if (player is null)
        {
            throw new EntityNotFoundException<Player>(name);
        }

        return player;
    }

    public async Task RemovePlayerById(Guid id)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == id);
        if (player is null)
        {
            throw new EntityNotFoundException<Player>(id);
        }

        context.Players.Remove(player);
        await context.SaveChangesAsync();
    }

    public async Task AddPlayerInSquad(Guid playerId, Guid squadId)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
        if (player is null)
        {
            throw new EntityNotFoundException<Player>(playerId);
        }

        var squad = await context.Squads.FirstOrDefaultAsync(p => p.Id == squadId);
        if (squad is null)
        {
            throw new EntityNotFoundException<Squad>(squadId);
        }

        player.SquadId = squadId;
        context.Update(player);
        await context.SaveChangesAsync();
    }

    public async Task RemovePlayerFromSquad(Guid playerId)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
        if (player is null)
        {
            throw new EntityNotFoundException<Player>(playerId);
        }

        if (player.SquadId is null)
        {
            throw new PlayerIsNotInSquad<Player>(playerId);
        }

        player.SquadId = null;
        context.Update(player);
        await context.SaveChangesAsync();
    }
}