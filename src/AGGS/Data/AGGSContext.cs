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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>().ToTable("Students");
        }
    }
}
