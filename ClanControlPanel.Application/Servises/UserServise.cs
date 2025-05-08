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
using Microsoft.EntityFrameworkCore;
using ClanControlPanel.Infrastructure.Mappings;

namespace ClanControlPanel.Application.Servises
{
    public class UserServise(IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, ClanControlPanelContext context) : IUserServices
    {
        public async Task<List<User>> GetUsers()
        {
            var userList = await context.Users.Select(u => u.ToDomain()).ToListAsync();
            return userList;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new Exception("Пользователь не найден");
            }

            return user.ToDomain();
        }

        public async Task<string> Login(string login, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null)
            {
                throw new Exception("Пользователь не найден"); 
            }
            var resultVerify = passwordHasher.Verify(password, user.PasswordHash);

            if (resultVerify)
            {
                return tokenGenerator.GenerateToken(user.ToDomain());
            }
            throw new Exception("Неверный логин или пароль"); 
        }

        public async Task<string?> Register(string login, string password, string name, string? role)
        {
            if (await context.Users.FirstOrDefaultAsync(u => u.Login == login) is not null)
            {
                throw new Exception("Логин занят");
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
                await context.Users.AddAsync(user.ToDb());
                await context.SaveChangesAsync();
                return login;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveUserById(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                throw new Exception("Пользователь не найден");
            }

            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUser(Guid id, string? name, string? login, string? password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                throw new Exception("Пользователь не найден");
            }

            if (!string.IsNullOrEmpty(name))
            {
                user.Name = name;
            }

            if (!string.IsNullOrEmpty(login))
            {
                if (await context.Users.FirstOrDefaultAsync(u => u.Login == login) is not null)
                {
                    throw new Exception("Логин занят");
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
                throw new Exception(ex.Message);
            }
        }
    }
}
