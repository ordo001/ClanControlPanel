using System.ComponentModel.DataAnnotations;
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
            var validationResult = validator.ValidationEntity(updateUserRequest);

            // Добавляем ручную валидацию логина
            if (!string.IsNullOrWhiteSpace(updateUserRequest.Login) && updateUserRequest.Login.Length < 2)
            {
                validationResult.Add(new ValidationResult("Минимальная длина логина — 2 символа", new[] { "Login" }));
            }

            // Добавляем ручную валидацию пароля
            if (!string.IsNullOrWhiteSpace(updateUserRequest.Password) && updateUserRequest.Password.Length < 2)
            {
                validationResult.Add(new ValidationResult("Минимальная длина пароля — 2 символа", new[] { "Password" }));
            }

            if (validationResult.Any())
            {
                return BadRequest(validationResult);
            }
            
            
            await userServise.UpdateUser(updateUserRequest.Id, updateUserRequest.Name, updateUserRequest.Login,
                updateUserRequest.Password);
            await hubContext.Clients.All.SendAsync("UsersUpdated");
            return Ok();
        }
    }
}