using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClanControlPanel.Infrastructure.Data;
using ClanControlPanel.Core.Models;
using ClanControlPanel.Core.DTO;

namespace ClanControlPanel.Infrastructure.Mappings
{
    public static class UserMappings
    {
        public static Core.Models.User ToDomain(this UserDb user)
        {
            var toDomain = new User(user.Login, user.PasswordHash, user.Name);

            return toDomain;
        }

        public static UserDb ToEntity(this Core.Models.User user)
        {
            var toEntity = new UserDb{
                Id = user.Id,
                Login = user.Login,
                PasswordHash = user.PasswordHash,
                Name = user.Name
                
            };

            return toEntity;
        }
    } 
}
