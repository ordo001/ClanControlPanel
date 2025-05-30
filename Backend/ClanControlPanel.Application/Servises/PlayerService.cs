using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using ClanControlPanel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Application.Servises;

public class PlayerService(IUserServices userService, ClanControlPanelContext context) : IPlayerService
{
    public async Task<List<Player>> GetPlayers()
    {
        var players = await context.Players
            .AsNoTracking()
            .Include(p => p.Squad)
            .ToListAsync();
        return players;
    }

    public async Task AddPlayer(string name, Guid? squadId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == name);
        if (user is null)
        {
            throw new EntityNotFoundException<User>(name);
        }

        user.Role = Role.Member;
        
        var maxPosition = await context.Players
            .Where(p => p.SquadId == squadId)
            .MaxAsync(p => p.Position) ?? 0;
        
        var player = new Player
        {
            Name = name,
            UserId = user.Id,
            SquadId = squadId,
            Position = maxPosition + 1
        };
        
        await context.Players.AddAsync(player);
        context.Users.Update(user);
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
        var player = await context.Players
            .FirstOrDefaultAsync(p => p.Id == id);
        if (player is null)
        {
            throw new EntityNotFoundException<Player>(id);
        }

        var user = await context.Users.SingleOrDefaultAsync(u => u.Id == player.UserId);
        if (user is null)
        {
            throw new EntityNotFoundException<User>(player.UserId);
        }

        user.Role = Role.User;
        context.Users.Update(user);
        context.Players.Remove(player);
        await context.SaveChangesAsync();
    }

    public async Task AddPlayerInSquad(Guid playerId, Guid squadId, int position)
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
        
        /*bool isSameSquad = player.SquadId == squadId;

        var maxPosition = await context.Players
            .Where(p => p.SquadId == squadId)
            .MaxAsync(p => (int?)p.Position) ?? 0;
        
        
        player.SquadId = squadId;
        player.Position = maxPosition + 1;
        context.Update(player);
        await context.SaveChangesAsync();*/
        
        var isSameSquad = player.SquadId == squadId;
        
        var playersInTargetSquad = await context.Players
            .Where(p => p.SquadId == squadId && p.Id != playerId)
            .OrderBy(p => p.Position)
            .ToListAsync();
        
        position = Math.Clamp(position, 0, playersInTargetSquad.Count);
        
        playersInTargetSquad.Insert(position, player);

        for (int i = 0; i < playersInTargetSquad.Count; i++)
        {
            playersInTargetSquad[i].Position = i;
        }

        player.SquadId = squadId;

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
        player.Position = null;
        context.Update(player);
        await context.SaveChangesAsync();
    }
}