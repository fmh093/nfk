using Microsoft.EntityFrameworkCore;
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

        // Additional DbSet properties for other entities can be added here.
    }
}
