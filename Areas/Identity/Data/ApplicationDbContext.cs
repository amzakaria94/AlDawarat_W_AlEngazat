using AlDawarat_W_AlEngazat.Models;
using AlDawarat_W_AlEngazat.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlDawarat_W_AlEngazat.Areas.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<PreviousCourse> PreviousCourses { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
