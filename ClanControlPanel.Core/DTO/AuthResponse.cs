using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanControlPanel.Core.DTO
{
    public class AuthResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        //public DateTime Expiration { get; set; }
    }
}
