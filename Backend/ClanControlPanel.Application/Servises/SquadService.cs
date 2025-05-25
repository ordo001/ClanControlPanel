using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.DTO.Response;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using ClanControlPanel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Application.Servises;

public class SquadService(ClanControlPanelContext context) : ISquadService
{
    public async Task UpdateNameSquad(Guid squadId, string name)
    {
        var squad = await context.Squads.FirstOrDefaultAsync(p => p.Id == squadId);
        if (squad is null)
        {
            throw new EntityNotFoundException<Squad>(squadId);
        }

        squad.NameSquad = name;
        context.Update(squad);
        await context.SaveChangesAsync();
    }

    public async Task<Squad> CreateSquad(string name)
    {
        var squad = new Squad
        {
            NameSquad = name
        };

        await context.Squads.AddAsync(squad);
        await context.SaveChangesAsync();
        return squad;
    }

    public async Task<List<SquadResponse>> GetPlayersInSquad(Guid squadId)
    {
        var squad = await context.Squads
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == squadId);
        if (squad is null)
        {
            throw new EntityNotFoundException<Squad>(squadId);
        }

        var players = await context.Players
            .AsNoTracking()
            .Where(p => p.SquadId == squadId).ToListAsync();
        return players.Select(p => new SquadResponse
        {
            IdPlayer = p.Id,
            IdSquad = p.SquadId,
            NamePlayer = p.Name,
            UserId = p.UserId
        }).ToList();
    }

    public async Task<List<SquadFullResponse>> GetSquads()
    {
        var squads = await context.Squads
            .AsNoTracking()
            .Include(p => p.Players)
            .ToListAsync();
        return squads.Select(s => new SquadFullResponse
        {
            Id = s.Id,
            NameSquad = s.NameSquad,
            Players = s.Players.Select(p => new PlayersResponse
            {
                Id = p.Id,
                Name = p.Name,
                Position = p.Position
            }).ToList()
        }).ToList();
    }
}