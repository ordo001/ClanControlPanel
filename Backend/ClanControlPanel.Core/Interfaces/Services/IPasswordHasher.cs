using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanControlPanel.Core.Interfaces.Services
{
    public interface IPasswordHasher
    {
        public string GenerateHash(string password);
        public bool Verify(string password, string passwordHash);
    }
}
