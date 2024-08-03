namespace MedicLab.Models.DTO
{
    public class dtoUserUpdate
    {
        public int Id { get; set; } 
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } =string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime LastLoginDate { get; set; }
    }
}
