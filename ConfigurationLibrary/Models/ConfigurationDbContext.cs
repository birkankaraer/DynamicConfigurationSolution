using Microsoft.EntityFrameworkCore;

namespace ConfigurationLibrary.Models
{
    public class ConfigurationDbContext : DbContext
    {
        public DbSet<ConfigurationRecord> ConfigurationRecords { get; set; }

        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationRecord>().ToTable("ConfigurationRecords");
        }
    }
}