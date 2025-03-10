using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlDawarat_W_AlEngazat.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<PreviousCourse> PreviousCourses { get; set; }
    }
}
