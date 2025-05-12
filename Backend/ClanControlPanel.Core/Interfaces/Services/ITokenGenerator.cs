using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Core.Interfaces.Services
{
    public interface ITokenGenerator
    {
        public string GenerateToken(User user);
    }
}
