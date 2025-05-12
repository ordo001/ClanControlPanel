using ClanControlPanel.Application.Exceptions;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /*[Authorize]*/
    public class UserController(IUserServices userServise, IValidatorService validator, IConfiguration config)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userList = await userServise.GetUsers();
            return Ok(userList);
        }

        [HttpDelete("/api/{userId}")]
        public async Task<IActionResult> RemoveUser(Guid userId)
        {
            await userServise.RemoveUserById(userId);
            return Ok();
        }

        [HttpPost]
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

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
        {
            await userServise.UpdateUser(updateUserRequest.Id, updateUserRequest.Name, updateUserRequest.Login,
                updateUserRequest.Password);
            return Ok();
        }
    }
}