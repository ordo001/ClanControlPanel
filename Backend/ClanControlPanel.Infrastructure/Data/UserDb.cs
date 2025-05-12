
namespace ClanControlPanel.Infrastructure.Data
{
    public class UserDb
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } 
        public PlayerDb? Player { get; set; }
    }
}
