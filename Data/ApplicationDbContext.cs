using Microsoft.EntityFrameworkCore;
using tracker.Models;

namespace tracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Diary> Diary { get; set; }
        public DbSet<Briefcase> Briefcases { get; set; }
        public DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>()
                .HasMany(a => a.Briefcases)
                .WithOne(b => b.Color)
                .HasForeignKey(b => b.ColorId);
        }
    }
}