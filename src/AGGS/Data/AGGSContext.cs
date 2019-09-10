using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AGGS.Models;

namespace AGGS.Data
{
    public class AGGSContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AGGSContext(DbContextOptions<AGGSContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Referral> Referrals { get; set; }
        public DbSet<StudentsClass> StudentsClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Teacher>().ToTable("Teachers");
            builder.Entity<Class>().ToTable("Classes");
            builder.Entity<Referral>().ToTable("Referrals");
            builder.Entity<StudentsClass>().ToTable("StudentsClasses").HasKey(c => new { c.StudentId, c.ClassId });
        }
    }
}
