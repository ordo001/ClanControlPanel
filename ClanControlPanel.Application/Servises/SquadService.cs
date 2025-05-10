using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using ClanControlPanel.Infrastructure.Data;
using ClanControlPanel.Infrastructure.Mappings;
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

        try
        {
            squad.NameSquad = name;
            context.Update(squad);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Squad> CreateSquad(string name)
    {
        try
        {
            var squad = new Squad
            {
                NameSquad = name
            };
            
            await context.Squads.AddAsync(squad);
            await context.SaveChangesAsync();
            return squad;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Player>> GetPlayersInSquad(Guid squadId)
    {
        var squad = await context.Squads.FirstOrDefaultAsync(p => p.Id == squadId);
        if (squad is null)
        {
            throw new EntityNotFoundException<Squad>(squadId);
        }
        
        try
        {
            var squads = await context.Players.Where(p => p.SquadId == squadId).ToListAsync();
            return squads;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<Squad>> GetSquads()
    {
        var squads = await context.Squads.ToListAsync();
        return squads;
    }
}