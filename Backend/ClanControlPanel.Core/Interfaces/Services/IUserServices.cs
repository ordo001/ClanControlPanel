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
        Task<string> Login(string login, string password);
        Task<string?> Register(string login, string password, string name, string? role);
        Task<List<User>> GetUsers();
        Task<User> GetUserById(Guid userId);
        Task RemoveUserById(Guid id);
        Task UpdateUser(Guid id, string? name, string? login, string? password);
    }
}
