using System;
using System.Collections.Generic;

namespace ClanControlPanel.Infrastructure.Data;

public partial class UserDb
{
    public int IdUser { get; set; }

    public string? Login { get; set; }

    public string? PasswordHash { get; set; }

    public string? Name { get; set; }
}
