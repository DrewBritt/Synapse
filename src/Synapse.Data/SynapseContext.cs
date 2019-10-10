using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Synapse.Data.Models;

namespace Synapse.Data
{
    public class SynapseContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public SynapseContext(DbContextOptions<SynapseContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Referral> Referrals { get; set; }
        public DbSet<StudentsClass> StudentsClasses { get; set; }
        public DbSet<IdentityUser> AspNetUsers { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentCategory> AssignmentCategories { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Teacher>().ToTable("Teachers");
            builder.Entity<Class>().ToTable("Classes");
            builder.Entity<Referral>().ToTable("Referrals");
            builder.Entity<StudentsClass>().ToTable("StudentsClasses").HasKey(c => new { c.StudentId, c.ClassId });
            builder.Entity<IdentityUser>().ToTable("aspnetusers");
            builder.Entity<Assignment>().ToTable("Assignments");
            builder.Entity<AssignmentCategory>().ToTable("AssignmentCategories");
            builder.Entity<Grade>().ToTable("Grades");
        }
    }
}
