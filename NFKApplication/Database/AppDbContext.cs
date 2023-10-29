using Microsoft.EntityFrameworkCore;
using NFKApplication.Database.Models;
using NFKApplication.Models;

namespace NFKApplication.Database
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<BasketDto> Baskets { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<LogEntryDto> Logs { get; set; }

        // Additional DbSet properties for other entities can be added here.
    }
}
