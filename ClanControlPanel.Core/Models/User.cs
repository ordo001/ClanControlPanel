using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanControlPanel.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        public Player? Player { get; set; }

        public User(string login, string password, string name) {
            Id = Guid.NewGuid();
            Login = login;
            PasswordHash = password;
            Name = name;
        }
        
        public User(Guid id, string login, string password, string name)
        {
            Id = id;
            Login = login;
            PasswordHash = password;
            Name = name;
        }
    }
}
