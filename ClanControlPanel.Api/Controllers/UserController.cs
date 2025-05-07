using ClanControlPanel.Application.Servises;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace WebApiСontrolPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserServices userServise, IConfiguration config) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {

            var userList = await userServise.GetAllUser();

            return Ok(userList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            await userServise.RemoveUserById(id);
            return Ok();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            var validationResult = registerUserRequest.Validation(registerUserRequest);
            if (validationResult.Any())
            {
                return BadRequest(validationResult);
            }

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

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
        {
            var task = userServise.UpdateUser(updateUserRequest.Id, updateUserRequest.Name, updateUserRequest.Login, updateUserRequest.Password);
            try
            {
                await task;
                //var result = Task.WhenAny(task);
                //Task.WhenAny(task);
                //if(result.Exception is not null)
                    //throw result.Exception.InnerException;
                return Ok();
            }
            catch
            {
                return BadRequest(task.Exception?.InnerException?.Message);
            }
            // TODO: Сделать обработку исключений, но до этого создать кастомные
            //return Ok();
        }
    }
}
