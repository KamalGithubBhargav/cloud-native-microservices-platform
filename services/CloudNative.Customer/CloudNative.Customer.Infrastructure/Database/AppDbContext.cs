using Microsoft.EntityFrameworkCore;

namespace CloudNative.Customer.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customers> Customers { get; set; }

      
    }
}
