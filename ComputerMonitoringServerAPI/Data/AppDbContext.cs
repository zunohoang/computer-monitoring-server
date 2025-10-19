using ComputerMonitoringServerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerMonitoringServerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ContestSbd> ContestSbds { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessBlacklist> ProcessBlacklists { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Violation> Violations { get; set; }
        public DbSet<ContestProcessBlacklist> ContestProcessBlacklists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Room>()
                .HasIndex(r => r.AccessCode)
                .IsUnique();

            modelBuilder.Entity<ContestSbd>()
                .HasIndex(c => c.Sbd)
                .IsUnique();

            modelBuilder.Entity<ProcessBlacklist>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // Configure indexes
            modelBuilder.Entity<Attempt>()
                .HasIndex(a => a.ContestId);

            modelBuilder.Entity<Process>()
                .HasIndex(p => p.Name);

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.Type);

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.AttemptId);

            // Configure foreign keys
            modelBuilder.Entity<Contest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("CreatedBy")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.CreatedByNavigation)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Violation>()
                .HasOne(v => v.HandledByNavigation)
                .WithMany()
                .HasForeignKey(v => v.HandledBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Violation>()
                .HasOne(v => v.CreatedByNavigation)
                .WithMany()
                .HasForeignKey(v => v.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
