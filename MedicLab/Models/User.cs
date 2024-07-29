using System.ComponentModel.DataAnnotations;

namespace MedicLab.Models
{
    public class User
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Orders { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string Role { get; set; } = string.Empty;
        public DateOnly LastLoginDate { get; set; }

    }
}
