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

        public DbSet<User> Users { get; set; }
        public DbSet<tracker.Models.Task> Tasks {  get; set; } 
        public DbSet<Diary> Diary { get; set; }
        public DbSet<Briefcase> Briefcases { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>()
                .HasMany(a => a.Briefcases)
                .WithOne(b => b.Color)
                .HasForeignKey(b => b.ColorId);

            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Briefcase)
                .WithMany(b => b.Task)
                .HasForeignKey(t => t.BriefcaseId);

            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Status)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.StatusId);

            modelBuilder.Entity<Models.Task>()
                .HasMany(t => t.Workers)
                .WithMany(w => w.Task)
                .UsingEntity(j => j.ToTable("TaskWorkers"));

            base.OnModelCreating(modelBuilder);
        }
    }
}