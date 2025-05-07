using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<User?> GetByLoginAsync(string login);
        Task<ICollection<UserResponse>> GetUsers();
        Task UpdateUser(User user, int id);
    }
}
