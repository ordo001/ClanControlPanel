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
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        public Role Role { get; set; }
        public Player? Player { get; set; }
    }
}
