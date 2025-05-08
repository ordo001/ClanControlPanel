using ClanControlPanel.Application.Servises;
using ClanControlPanel.Core.Interfaces.Services;
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
            try
            {
                var players = await playerService.GetPlayers();
                return Ok(players);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] Guid userId, string name)
        {
            try
            {
                await playerService.AddPlayer(userId, name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetPlayers(Guid playerId)
        {
            try
            {
                var player = await playerService.GetPlayerById(playerId);
                return Ok(player);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(Guid playerId)
        {
            try
            {
                await playerService.RemovePlayerById(playerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("/Squads/{squadId}/Players/{playerId}")]
        public async Task<IActionResult> AddPlayerInSquad(Guid squadId, Guid playerId)
        {
            try
            {
                await playerService.AddPlayerInSquad(playerId, squadId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("/Squads/{squadId}/Players/{playerId}")]
        public async Task<IActionResult> RemovePlayerFromSquad(Guid squadId, Guid playerId)
        {
            try
            {
                await playerService.RemovePlayerFromSquad(playerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
