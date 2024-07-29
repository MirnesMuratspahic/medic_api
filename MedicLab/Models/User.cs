using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedicLab.Models
{
    //id, name, username, orders, last login date, image, status, date of birth.
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [JsonIgnore][Required]
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
        public string Status {  get; set; } = string.Empty;
        [JsonIgnore][Required]
        public string Role { get; set; } = string.Empty;
        public DateOnly LastLoginDate { get; set; }

    }
}
