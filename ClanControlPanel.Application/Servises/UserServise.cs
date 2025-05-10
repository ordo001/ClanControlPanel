using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using ClanControlPanel.Infrastructure.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClanControlPanel.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using ClanControlPanel.Infrastructure.Mappings;

namespace ClanControlPanel.Application.Servises
{
    public class UserServise(IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, ClanControlPanelContext context) : IUserServices
    {
        public async Task<List<User>> GetUsers()
        {
            var userList = await context.Users.ToListAsync();
            return userList;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new EntityNotFoundException<User>(userId);
            }

            return user;
        }

        public async Task<string> Login(string login, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null)
            {
                throw new EntityNotFoundException<User>(login);
            }
            var resultVerify = passwordHasher.Verify(password, user.PasswordHash);

            if (resultVerify)
            {
                return tokenGenerator.GenerateToken(user);
            }
            throw new Exception("Неверный логин или пароль"); 
        }

        public async Task<string?> Register(string login, string password, string name, string? role)
        {
            if (await context.Users.FirstOrDefaultAsync(u => u.Login == login) is not null)
            {
                throw new EntityIsExists<User>(login);
            }
            var passwordHash = passwordHasher.GenerateHash(password);

            var user = new User
            {
                Login = login,
                PasswordHash = passwordHash,
                Name = name,
                Role = role ?? "User"
            };
            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return login;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task RemoveUserById(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                throw new EntityNotFoundException<User>(id);
            }

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateUser(Guid id, string? name, string? login, string? password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                throw new EntityNotFoundException<User>(id);
            }

            if (!string.IsNullOrEmpty(name))
            {
                user.Name = name;
            }

            if (!string.IsNullOrEmpty(login))
            {
                if (await context.Users.FirstOrDefaultAsync(u => u.Login == login) is not null)
                {
                    throw new EntityIsExists<User>(login);
                }
                user.Login = login;
            }

            if (!string.IsNullOrEmpty(password))
            {
                var NewPasswordHash = passwordHasher.GenerateHash(password);
                user.PasswordHash = NewPasswordHash;
            }

            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
