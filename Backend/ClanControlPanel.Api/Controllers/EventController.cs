using ClanControlPanel.Api.Hubs;
using ClanControlPanel.Application.Exceptions;
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
    [Authorize(Roles = "Member, Moder, Admin")]
    [ApiController]
    public class EventController(IEventService eventService, IHubContext<EventHub> hubContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await eventService.GetEvents();
            return Ok(events);
        }

        [HttpGet("/api/Event/Types")]
        public async Task<IActionResult> GetEventTypes()
        {
            var eventTypes = await eventService.GetEventTypes();
            return Ok(eventTypes);
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
            var eventId = await eventService.AddEvent(eventAddRequest.Date, eventAddRequest.EventTypeId, eventAddRequest.Status);
            await hubContext.Clients.All.SendAsync("EventUpdated");
            return Ok(eventId);
        }

        [HttpDelete("{eventId:guid}")]
        public async Task<IActionResult> RemoveEvent(Guid eventId)
        {
            await eventService.RemoveEvent(eventId);
            await hubContext.Clients.All.SendAsync("EventUpdated");
            return Ok();
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
            await hubContext.Clients.All.SendAsync("AttendanceUpdated");
            return Ok();
        }
        
        [HttpPatch("{eventId:guid}/attendances/{playerId:guid}")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> UpdateAttendance(Guid eventId, Guid playerId,
            [FromBody] AttendanceUpdateRequest status)
        {
            await eventService.SetAttendance(eventId, playerId, status.Status);
            await hubContext.Clients.All.SendAsync("AttendanceUpdated");
            return Ok();
        }


        [HttpPost("{eventId:guid}/attendances/players/present")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> MarkPlayersPresent(Guid eventId, [FromBody] PlayerListRequest dto)
        {
            await eventService.MarkPlayersPresent(eventId, dto.PlayerIds);
            await hubContext.Clients.All.SendAsync("AttendanceUpdated");
            return Ok();
        }

        [HttpDelete("{eventId:guid}/attendances/{playerId:guid}")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> RemoveAttendance(Guid eventId, Guid playerId)
        {
            await eventService.RemoveAttendance(eventId, playerId);
            await hubContext.Clients.All.SendAsync("AttendanceUpdated");
            return Ok();
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
            await hubContext.Clients.All.SendAsync("EventUpdated");
            return Ok();
        }

        [HttpPut("stages/{stageId:guid}")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> UpdateStage(Guid stageId, [FromBody] EventStageUpdateRequest dto)
        {
            await eventService.UpdateEventStage(stageId, dto.Amount, dto.Description);
            await hubContext.Clients.All.SendAsync("EventUpdated");
            return Ok();
        }

        [HttpDelete("stages/{stageId:guid}")]
        [Authorize(Roles = "Moder, Admin")]
        public async Task<IActionResult> DeleteStage(Guid stageId)
        {
            await eventService.RemoveEventStage(stageId);
            await hubContext.Clients.All.SendAsync("EventUpdated");
            return Ok();
        }
    }
}