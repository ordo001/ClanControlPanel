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
            await squadService.CreateSquad(name);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSquads()
        {
            var squads = await squadService.GetSquads();
            return Ok(squads);
        }

        [HttpGet("/api/Squads/{squadId}/Players")]
        public async Task<IActionResult> GetSquads(Guid squadId)
        {
            var players = await squadService.GetPlayersInSquad(squadId);
            return Ok(players);
        }
    }
}