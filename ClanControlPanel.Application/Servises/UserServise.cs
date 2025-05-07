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

        public async Task<string?> Register(string login, string password, string name)
        {
            if (await context.Users.FirstOrDefaultAsync(u => u.Login == login) is not null)
            {
                throw new Exception("Логин занят");
            }
            var passwordHash = passwordHasher.GenerateHash(password);

            var user = new User(login, passwordHash, name);

            await context.Users.AddAsync(user.ToEntity());
            return login;
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
