using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClanControlPanel.Core.DTO
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Id обязателен")]
        public Guid Id { get; set; }
        public string? Login { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
    }
}
