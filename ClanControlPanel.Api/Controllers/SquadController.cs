using ClanControlPanel.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SquadController(ISquadService squadService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateSquad([FromBody] string name)
        {
            try
            {
                await squadService.CreateSquad(name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetSquads()
        {
            try
            {
                var squads = await squadService.GetSquads();
                return Ok(squads);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("/api/Players/{squadId}")]
        public async Task<IActionResult> GetSquads(Guid squadId)
        {
            try
            {
                var players = await squadService.GetPlayersInSquad(squadId);
                return Ok(players);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
