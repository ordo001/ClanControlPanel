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
            try
            {
                var players = await playerService.GetPlayers();
                
                return Ok(players.Select(p => new PlayersResponse
                {
                    Id = p.Id,
                    Name = p.Name
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerAddRequest playerAddRequest)
        {
            try
            {
                await playerService.AddPlayer(playerAddRequest.UserId, playerAddRequest.Name);
                return Ok();
            }
            catch (EntityNotFoundException<Player> ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("/api/Players/{playerId}")]
        public async Task<IActionResult> GetPlayers(Guid playerId)
        {
            try
            {
                var player = await playerService.GetPlayerById(playerId);
                return Ok(new PlayersResponse
                {
                    Id = player.Id,
                    Name = player.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("/api/Players/{playerId}")]
        public async Task<IActionResult> DeletePlayer(Guid playerId)
        {
            try
            {
                await playerService.RemovePlayerById(playerId);
                return Ok();
            }
            catch (EntityNotFoundException<Player> ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("/api/Squads/{squadId}/Players/{playerId}")]
        public async Task<IActionResult> AddPlayerInSquad(Guid squadId, Guid playerId)
        {
            try
            {
                await playerService.AddPlayerInSquad(playerId, squadId);
                return Ok();
            }
            catch (EntityNotFoundException<Player> ex)
            {
                return NotFound(ex.Message);
            }
            catch (EntityNotFoundException<Squad> ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("/api/Squads/{squadId}/Players/{playerId}")]
        public async Task<IActionResult> RemovePlayerFromSquad(Guid squadId, Guid playerId)
        {
            try
            {
                await playerService.RemovePlayerFromSquad(playerId);
                return Ok();
            }
            catch (EntityNotFoundException<Player> ex)
            {
                return NotFound(ex.Message);
            }
            catch (PlayerIsNotInSquad<Player> ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
