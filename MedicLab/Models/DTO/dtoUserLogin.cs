using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MedicLab.Models.DTO
{
    public class dtoUserLogin
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
