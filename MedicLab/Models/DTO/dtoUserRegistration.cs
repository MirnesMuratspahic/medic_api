using System.ComponentModel.DataAnnotations;

namespace MedicLab.Models.DTO
{

    //username, password, name, orders, image URL (public), date of birth (date picker).
    public class dtoUserRegistration
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Orders { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
