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
        try
        {
            var players = await context.Players.ToListAsync();
            return players.Select(p => p.ToDomain()).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task AddPlayer(Guid userId, string name)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            throw new Exception("Пользователь не найден");
        }

        try
        {
            var player = new PlayerDb
            {
                Name = name,
                UserId = userId
            };
            await context.Players.AddAsync(player);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Player> GetPlayerById(Guid id)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == id);
        if (player is null)
        {
            throw new Exception("Игрок не найден");
        }
        return player.ToDomain(); 
    }

    public async Task RemovePlayerById(Guid id)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == id);
        if (player is null)
        {
            throw new Exception("Игрок не найден");
        }

        try
        {
            context.Players.Remove(player);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task AddPlayerInSquad(Guid playerId, Guid squadId)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
        if (player is null)
        {
            throw new Exception("Игрок не найден");
        }
        var squad = await context.Squads.FirstOrDefaultAsync(p => p.Id == squadId);
        if (squad is null)
        {
            throw new Exception("Отряд не найден");
        }

        try
        {
            player.SquadId = squadId;
            context.Update(player);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }

    public async Task RemovePlayerFromSquad(Guid playerId)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
        if (player is null)
        {
            throw new Exception("Игрок не найден");
        }

        if (player.SquadId is null)
        {
            throw new Exception("Игрок не состоит в отряде");
        }

        try
        {
            player.SquadId = null;
            context.Update(player);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}