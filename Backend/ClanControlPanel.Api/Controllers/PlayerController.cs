using ClanControlPanel.Api.Hubs;
using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Application.Servises;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Moder, Admin")]
    public class PlayerController(IPlayerService playerService, IHubContext<PlayerHub> hubContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await playerService.GetPlayers();

            return Ok(players.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                SquadId = p.SquadId,
                SquadName = p.Squad is null ? "" : p.Squad.NameSquad
            }));
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerAddRequest playerAddRequest)
        {
            await playerService.AddPlayer(playerAddRequest.Name, playerAddRequest.SquadId);
            await hubContext.Clients.All.SendAsync("PlayersUpdated");
            return Ok();
        }

        [HttpGet("/api/Players/{playerId}")]
        public async Task<IActionResult> GetPlayers(Guid playerId)
        {
            var player = await playerService.GetPlayerById(playerId);
            return Ok(new PlayersResponse
            {
                Id = player.Id,
                Name = player.Name
            });
        }

        [HttpDelete("/api/Players/{playerId}")]
        public async Task<IActionResult> DeletePlayer(Guid playerId)
        {
            await playerService.RemovePlayerById(playerId);
            await hubContext.Clients.All.SendAsync("PlayersUpdated");
            return Ok();
        }

        [HttpPost("/api/Squads/{squadId}/Players/{playerId}")]
        public async Task<IActionResult> AddPlayerInSquad(Guid squadId, Guid playerId, [FromQuery] int position)
        {
            await playerService.AddPlayerInSquad(playerId, squadId, position);
            await hubContext.Clients.All.SendAsync("PlayersUpdated");
            return Ok();
        }

        [HttpDelete("/api/Squads/{squadId}/Players/{playerId}")]
        public async Task<IActionResult> RemovePlayerFromSquad(Guid squadId, Guid playerId)
        {
            await playerService.RemovePlayerFromSquad(playerId);
            await hubContext.Clients.All.SendAsync("PlayersUpdated");
            return Ok();
        }
    }
}