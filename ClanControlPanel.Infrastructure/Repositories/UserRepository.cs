using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClanControlPanel.Infrastructure.Data;
using ClanControlPanel.Infrastructure.Mappings;
using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces;
using ClanControlPanel.Core.Interfaces.Repositories;
using ClanControlPanel.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ControlPanelContext _context;
        public UserRepository(ControlPanelContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.AddAsync(user.MapToEntity());
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.IdUser == id);
            if (result != null)
            {
                _context.Users.Remove(result);
                await _context.SaveChangesAsync();
            }
            //throw new NotImplementedException();
            // Exception  
        }

        public async Task<ClanControlPanel.Core.Models.User?> GetByIdAsync(int id)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.IdUser == id);
            if (result != null)
                return UserMappings.MapToDto(result);
            else
                return null;
        }

        public async Task<ClanControlPanel.Core.Models.User?> GetByLoginAsync(string login)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (result != null)
                return result.MapToDto();
            else
                return null;
        }

        public async Task<ICollection<UserResponse>> GetUsers()
        {
            var result = await _context.Users.AsNoTracking().ToListAsync();

            return result.MapToDtoListUserResponse();
        }

        public async Task UpdateUser(User user, int id)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id);
            userDb.Login = user.Login;
            userDb.PasswordHash = user.PasswordHash;
            userDb.Name = user.Name;
            _context.Users.Update(userDb);
            await _context.SaveChangesAsync();
        }
    }
}
