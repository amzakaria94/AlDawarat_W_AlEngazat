using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlDawarat_W_AlEngazat.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Course> Courses { get; set; }
    }
}