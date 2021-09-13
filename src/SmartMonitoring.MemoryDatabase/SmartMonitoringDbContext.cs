using Microsoft.EntityFrameworkCore;
using SmartMonitoring.MemoryDatabase.Entities;

namespace SmartMonitoring.MemoryDatabase
{
    public class SmartMonitoringDbContext : DbContext
    {
        public SmartMonitoringDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>(serviceBuilder =>
            {
                serviceBuilder.HasIndex(service => service.Name).IsUnique();
                serviceBuilder.HasMany(service => service.Labels);
            });
        }
    }
}
