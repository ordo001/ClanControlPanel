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
            throw new Exception("Отряд не найден");
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
            var squad = new SquadDb
            {
                NameSquad = name
            };
            
            await context.Squads.AddAsync(squad);
            await context.SaveChangesAsync();
            return squad.ToDomain();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}