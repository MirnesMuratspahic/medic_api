namespace MedicLab.Models.DTO
{

    //id, name, username, last login date.
    public class dtoUserInformation
    {
        public  int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public DateOnly LastLoginDate { get; set; }
    }
}
