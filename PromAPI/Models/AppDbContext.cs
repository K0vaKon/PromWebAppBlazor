using PhotoPromAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PromAPI.Models
{
    //NuGetPackage: Microsoft.EntityFrameworkCore (als niet werkt)
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Photo> Photos { get; set; }
    }
}
