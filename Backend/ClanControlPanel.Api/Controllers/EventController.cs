using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Member, Moder, Admin")]
    [ApiController]
    public class EventController(IEventService eventService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await eventService.GetEvents();
            return Ok(events);
        }

        [HttpGet("{eventId:guid}")]
        public async Task<IActionResult> GetEventById(Guid eventId)
        {
            var ev = await eventService.GetEventById(eventId);
            return Ok(ev);
        }

        [HttpPost]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> AddEvent([FromBody] EventAddRequest eventAddRequest)
        {
            await eventService.AddEvent(eventAddRequest.Date, eventAddRequest.EventTypeId, eventAddRequest.Status);
            return Ok();
        }

        [HttpDelete("{eventId:guid}")]
        public async Task<IActionResult> RemoveEvent(Guid eventId)
        {
            await eventService.RemoveEvent(eventId);
            return NoContent();
        }

        [HttpGet("/api/Events/Attendances/Player/{playerId}")]
        public async Task<IActionResult> GetPlayerAttendances(Guid playerId)
        {
            var list = await eventService.GetPlayerAttendance(playerId);
            return Ok(list);
        }

        [HttpGet("/api/Events/{eventId}/Attendances")]
        public async Task<IActionResult> GetEventAttendances(Guid eventId)
        {
            var attendances = await eventService.GetEventAttendance(eventId);
            return Ok(attendances);
        }

        [HttpPost("{eventId:guid}/attendances/{playerId:guid}")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> SetAttendance(Guid eventId, Guid playerId,
            [FromBody] AttendanceUpdateRequest dto)
        {
            await eventService.SetAttendance(eventId, playerId, dto.Status, dto.AbsenceReason);
            return NoContent();
        }

        [HttpPost("{eventId:guid}/attendances/players/present")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> MarkPlayersPresent(Guid eventId, [FromBody] PlayerListRequest dto)
        {
            await eventService.MarkPlayersPresent(eventId, dto.PlayerIds);
            return NoContent();
        }

        [HttpDelete("{eventId:guid}/attendances/{playerId:guid}")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> RemoveAttendance(Guid eventId, Guid playerId)
        {
            await eventService.RemoveAttendance(eventId, playerId);
            return NoContent();
        }
        
        [HttpGet("{eventId:guid}/stages")]
        public async Task<IActionResult> GetEventStages(Guid eventId)
        {
            var stages = await eventService.GetEventStages(eventId);
            return Ok(stages);
        }

        [HttpPost("{eventId:guid}/stages")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> AddStage(Guid eventId, [FromBody] EventStageAddRequest dto)
        {
            await eventService.AddEventStage(eventId, dto.StageNumber, dto.Amount, dto.Description);
            return Ok();
        }

        [HttpPut("stages/{stageId:guid}")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> UpdateStage(Guid stageId, [FromBody] EventStageUpdateRequest dto)
        {
            await eventService.UpdateEventStage(stageId, dto.Amount, dto.Description);
            return NoContent();
        }

        [HttpDelete("stages/{stageId:guid}")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> DeleteStage(Guid stageId)
        {
            await eventService.RemoveEventStage(stageId);
            return NoContent();
        }
    }
}