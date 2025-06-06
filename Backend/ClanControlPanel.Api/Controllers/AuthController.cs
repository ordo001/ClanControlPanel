using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ClanControlPanel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserServices userServices) : ControllerBase
    {
        [HttpPost("/api/Auth")]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            var token = await userServices.Login(authRequest.Login, authRequest.Password);
            if (token is null)
            {
                return Unauthorized();
            }

            /*Response.Cookies.Append("JwtMonster",token);*/

            Response.Cookies.Append("JwtMonster", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Вырубить для http 
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
            
            //Response.Cookies.Append("JwtMonster", token);
            
            return Ok(new {
                token = token
            });
        }

        [HttpGet("/api/Auth/Validate-token")]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            var user = await userServices.GetUserById(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
            return Ok(new
            {
                isValid = true,
                user = new
                {
                    id = user.Id,
                    login = user.Login,
                    name = user.Name,
                    role = user.Role
                }
            });
        }

        [HttpPost("/api/Auth/Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("JwtMonster");
            // Реализовать сервис по добавлению токена в черный список, для избежания повторного его использования
            return Ok();
        }


       
    }
}
