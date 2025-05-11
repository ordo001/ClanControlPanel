using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserServices userServices) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            var token = await userServices.Login(authRequest.Login, authRequest.Password);
            if (token is null)
            {
                return NotFound();
            }

            HttpContext.Response.Cookies.Append("JwtMonster",token);

            return Ok(token);
        }

        [HttpGet("/api/validate-token")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            return Ok(new {isValid = true});
        }

        [HttpPost("/api/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("JwtMonster");
            // Реализовать сервис по добавлению токена в черный список, для избежания повторного его использования
            return Ok();
        }


       
    }
}
