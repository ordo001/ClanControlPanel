using ClanControlPanel.Core.DTO;
using ClanControlPanel.Core.Interfaces.Repositories;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanControlPanel.Application.Servises
{
    public class UserServise : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        public UserServise(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;

        }

        public async Task<ICollection<UserResponse>> GetAllUser()
        {
            var userList = await _userRepository.GetUsers();

            return userList;
        }

        public async Task<string?> Login(string login, string password)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
            {
                // Мб потом Exception
                return null;
            }
            var resultVerify = _passwordHasher.Verify(password, user.PasswordHash);

            if (resultVerify)
            {
                return _tokenGenerator.GenerateToken(user);
            }
            // Мб потом Exception
            return null;
        }

        public async Task<string?> Register(string login, string password, string name)
        {
            if (await _userRepository.GetByLoginAsync(login) is not null)
            {
                return null;
            }
            var passwordHash = _passwordHasher.GenerateHash(password);

            var user = new User(login, passwordHash, name);

            await _userRepository.AddAsync(user);
            return login;
        }

        public async Task RemoveUserById(int id)
        {
            await _userRepository.DeleteAsync(id);

        }

        public async Task UpdateUser(int id, string? name, string? login, string? password)
        {
            var user = await _userRepository.GetByIdAsync(id);

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
                var result = await _userRepository.GetByLoginAsync(login);
                if (result is not null)
                    throw new Exception("Логин занят");
                user.Login = login;
            }

            if (!string.IsNullOrEmpty(password))
            {
                var NewPasswordHash = _passwordHasher.GenerateHash(password);
                user.PasswordHash = NewPasswordHash;

            }
            await _userRepository.UpdateUser(user, id);
        }
    }
}
