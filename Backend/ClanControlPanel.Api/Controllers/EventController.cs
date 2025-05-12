using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(IEventService eventService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var events = await eventService.GetEvents();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] EventAddRequest eventAddRequest)
        {
            await eventService.AddEvent(eventAddRequest.Date, eventAddRequest.EventTypeId, eventAddRequest.Status);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveEvent([FromBody] EventAddRequest eventAddRequest)
        {
            await eventService.AddEvent(eventAddRequest.Date, eventAddRequest.EventTypeId, eventAddRequest.Status);
            return Ok();
        }

        [HttpGet("/api/Events/Attendances/Player/{playerId}")]
        public async Task<IActionResult> GetPlayerAttendances(Guid playerId)
        {
            var attendances = await eventService.GetPlayerAttendance(playerId);
            return Ok(attendances);
        }

        [HttpGet("/api/Events/{eventId}/Attendances")]
        public async Task<IActionResult> GetEventAttendances(Guid eventId)
        {
            var attendances = await eventService.GetEventAttendance(eventId);
            return Ok(attendances);
        }

        [HttpPost("/api/Events/{eventId}/Attendances/Player/{playerId}")]
        public async Task<IActionResult> MarkPlayerInEvent(Guid eventId, Guid playerId)
        {
            await eventService.MarkPlayerInEvent(eventId, playerId);
            return Ok();
        }

        [HttpPost("/api/Events/{eventId}/Attendances/Players")]
        public async Task<IActionResult> MarkPlayerListInEvent(Guid eventId, [FromBody] List<string> playerName)
        {
            await eventService.MarkListPlayersInEvent(eventId, playerName);
            return Ok();
        }
    }
}