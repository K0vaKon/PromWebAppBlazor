using Microsoft.EntityFrameworkCore;
using PhotoPromAPI.Models;
using PromAPI.Models;

namespace PromAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // === ВОТ ЭТИХ СТРОК НЕ ХВАТАЕТ ===
        // This tells EF Core that these tables exist
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Zender> Zenders { get; set; }
        // =================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map C# classes to your exact MySQL table names
            modelBuilder.Entity<Photo>().ToTable("fotos");
            modelBuilder.Entity<Zender>().ToTable("zender");
        }
    }
}
