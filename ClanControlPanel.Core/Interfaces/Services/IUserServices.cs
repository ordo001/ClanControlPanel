using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Interfaces.Services
{
    public interface IUserServices
    {
        Task<string?> Login(string login, string password);
        Task<string?> Register(string login, string password, string name);
        Task<ICollection<UserResponse>> GetAllUser();
        Task RemoveUserById(int id);
        Task UpdateUser(int id, string name, string login, string password);
    }
}
