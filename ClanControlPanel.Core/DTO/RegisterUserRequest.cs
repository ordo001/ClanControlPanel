using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanControlPanel.Core.DTO
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "Укажите логин"), MinLength(3, ErrorMessage = "Минимальная длинна логина - 3 символа")]
        public string Login { get; set; } = string.Empty;
        [Required(ErrorMessage = "Укажите Пароль"), MinLength(4,ErrorMessage = "Минимальная длинна пароля - 4 символа")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Укажите имя"), MinLength(2, ErrorMessage = "Минимальная длинна имени - 2 символа")]
        public string Name { get; set; } = string.Empty;
    }
}
