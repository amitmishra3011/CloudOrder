using CloudOrder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudOrder.Infrastructure.Persistence
{
    public class CloudOrderDbContext : DbContext
    {
        public CloudOrderDbContext(DbContextOptions<CloudOrderDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
    }
}