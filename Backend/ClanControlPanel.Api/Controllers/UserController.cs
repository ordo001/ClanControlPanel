using ClanControlPanel.Api.Hubs;
using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController(IUserServices userServise,
        IValidatorService validator,
        IConfiguration config,
        IHubContext<UserHub> hubContext)
        : ControllerBase
    {
        [HttpGet("/api/Users")]
        public async Task<IActionResult> GetUsers()
        {
            var userList = await userServise.GetUsers();
            return Ok(userList);
        }

        [HttpDelete("/api/Users/{userId}")]
        public async Task<IActionResult> RemoveUser(Guid userId)
        {
            await userServise.RemoveUserById(userId);
            await hubContext.Clients.All.SendAsync("UsersUpdated");
            return Ok();
        }

        [HttpPost("/api/Users")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            var validationResult = validator.ValidationEntity(registerUserRequest);
            if (validationResult.Any())
            {
                return BadRequest(validationResult);
            }

            var result = await userServise.Register(
                registerUserRequest.Login,
                registerUserRequest.Password,
                registerUserRequest.Name,
                registerUserRequest.Role!);
            await hubContext.Clients.All.SendAsync("UsersUpdated");
            return Ok(result);
        }

        [HttpPatch("/api/Users")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
        {
            await userServise.UpdateUser(updateUserRequest.Id, updateUserRequest.Name, updateUserRequest.Login,
                updateUserRequest.Password);
            await hubContext.Clients.All.SendAsync("UsersUpdated");
            return Ok();
        }
    }
}