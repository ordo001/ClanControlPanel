using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ClanControlPanel.Application.Settings;
using Microsoft.IdentityModel.Tokens;

namespace ClanControlPanel.Application.Servises
{
    public class TokenGenerator(IOptions<AuthSettings> options) : ITokenGenerator
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Login", user.Login),
                new Claim("Name", user.Name)
            };
            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(options.Value.Expires),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(options.Value.SecretKey)), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
