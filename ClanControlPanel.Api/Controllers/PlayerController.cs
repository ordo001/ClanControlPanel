using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Application.Servises;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController(IPlayerService playerService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await playerService.GetPlayers();

            return Ok(players.Select(p => new PlayersResponse
            {
                Id = p.Id,
                Name = p.Name
            }));
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerAddRequest playerAddRequest)
        {
            await playerService.AddPlayer(playerAddRequest.UserId, playerAddRequest.Name);
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
            return Ok();
        }

        [HttpPost("/api/Squads/{squadId}/Players/{playerId}")]
        public async Task<IActionResult> AddPlayerInSquad(Guid squadId, Guid playerId)
        {
            await playerService.AddPlayerInSquad(playerId, squadId);
            return Ok();
        }

        [HttpDelete("/api/Squads/{squadId}/Players/{playerId}")]
        public async Task<IActionResult> RemovePlayerFromSquad(Guid squadId, Guid playerId)
        {
            await playerService.RemovePlayerFromSquad(playerId);
            return Ok();
        }
    }
}