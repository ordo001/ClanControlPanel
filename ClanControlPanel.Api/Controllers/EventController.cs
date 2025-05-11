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
            try
            {
                await eventService.AddEvent(eventAddRequest.Date, eventAddRequest.EventTypeId, eventAddRequest.Status);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete]
        public async Task<IActionResult> RemoveEvent([FromBody] EventAddRequest eventAddRequest)
        {
            try
            {
                await eventService.AddEvent(eventAddRequest.Date, eventAddRequest.EventTypeId, eventAddRequest.Status);
                return Ok(); 
            }
            catch (EntityNotFoundException<Event> ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("/api/Events/Attendances/Player/{playerId}")]
        public async Task<IActionResult> GetPlayerAttendances(Guid playerId)
        {
            try
            {
                var attendances = await eventService.GetPlayerAttendance(playerId);
                return Ok(attendances);
            }
            catch (EntityNotFoundException<Player> ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        
        [HttpGet("/api/Events/{eventId}/Attendances")]
        public async Task<IActionResult> GetEventAttendances(Guid eventId)
        {
            try
            {
                var attendances = await eventService.GetEventAttendance(eventId);
                return Ok(attendances);
            }
            catch (EntityNotFoundException<Event> ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpPost("/api/Events/{eventId}/Attendances/Player/{playerId}")]
        public async Task<IActionResult> MarkPlayerInEvent(Guid eventId, Guid playerId)
        {
            try
            {
                await eventService.MarkPlayerInEvent(eventId, playerId);
                return Ok();
            }
            catch (EntityNotFoundException<Event> ex)
            {
                return NotFound(ex.Message); 
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
        
        [HttpPost("/api/Events/{eventId}/Attendances/Players")]
        public async Task<IActionResult> MarkPlayerListInEvent(Guid eventId, [FromBody] List<string> playerName)
        {
            try
            {
                await eventService.MarkListPlayersInEvent(eventId, playerName);
                return Ok();
            }
            catch (EntityNotFoundException<Event> ex)
            {
                return NotFound(ex.Message); 
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
    }
}
