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
        public static User MapToDto(this UserDb user)
        {
            var ToDTO = new User(user.Login, user.PasswordHash, user.Name);

            return ToDTO;
        }

        public static UserDb MapToEntity(this User user)
        {
            var ToEntity = new UserDb
            {
                Login = user.Login,
                PasswordHash = user.PasswordHash,
                Name = user.Name
            };

            return ToEntity;
        }

        public static List<User> MapToDtoList(this List<UserDb> userList)
        {
            return userList.Select(user => user.MapToDto()).ToList();
        }

        public static UserResponse MapToDtoUserResponse(this UserDb user)
        {
            var ToDTO = new UserResponse
            {
                IdUser = user.IdUser,
                Login = user.Login,
                Name = user.Name
            };
            return ToDTO;
        }

        public static List<UserResponse> MapToDtoListUserResponse(this List<UserDb> userList)
        {
            return userList.Select(user => user.MapToDtoUserResponse()).ToList();
        }
    } 
}
