using Microsoft.EntityFrameworkCore;
using MedicLab.Models;

namespace MedicLab.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {  get; set; }

    }

}
