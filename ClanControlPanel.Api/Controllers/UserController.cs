using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserServices userServise, IValidatorService validator, IConfiguration config) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {

            try
            {
                var userList = await userServise.GetUsers();
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            try
            {
                await userServise.RemoveUserById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            var validationResult = validator.ValidationEntity(registerUserRequest);
            if (validationResult.Any())
            {
                return BadRequest(validationResult);
            }

            try
            {
                var result = await userServise.Register(
                    registerUserRequest.Login,
                    registerUserRequest.Password,
                    registerUserRequest.Name);

                if (result is null)
                {
                    return BadRequest("Логин занят");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                await userServise.UpdateUser(updateUserRequest.Id, updateUserRequest.Name, updateUserRequest.Login, updateUserRequest.Password);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
