using Microsoft.EntityFrameworkCore;
using CloudOrder.Domain.Entities;

namespace CloudOrder.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(CloudOrderDbContext context)
        {
            // Use EF Core async extensions
            if (await context.Customers.AnyAsync()
                || await context.Products.AnyAsync()
                || await context.Orders.AnyAsync())
            {
                return;
            }

            // Seed customers
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), Name = "Acme Corp", Email = "sales@acme.test" },
                new Customer { Id = Guid.NewGuid(), Name = "Contoso Ltd", Email = "info@contoso.test" },
                new Customer { Id = Guid.NewGuid(), Name = "Fabrikam", Email = "contact@fabrikam.test" }
            };
            await context.Customers.AddRangeAsync(customers);

            // Seed products
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Widget A", Price = 9.99m },
                new Product { Id = Guid.NewGuid(), Name = "Widget B", Price = 19.99m },
                new Product { Id = Guid.NewGuid(), Name = "Gadget",   Price = 29.99m }
            };
            await context.Products.AddRangeAsync(products);

            // Save customers & products first so we can reference their IDs in orders
            await context.SaveChangesAsync();

            // Seed orders (simple examples referencing seeded customers)
            var orders = new List<Order>
            {
                new Order
                {
                    ID = Guid.NewGuid(),
                    CustomerId = customers[0].Id,
                    ProductId=products[0].Id,
                    OrderDate = DateTime.UtcNow,
                    Status = "Created"
                },
                new Order
                {
                    ID = Guid.NewGuid(),
                    CustomerId = customers[1].Id,
                    ProductId=products[1].Id,
                    OrderDate = DateTime.UtcNow.AddMinutes(-30),
                    Status = "In Progress"
                },
                new Order
                {
                    ID = Guid.NewGuid(),
                    CustomerId = customers[2].Id,
                    ProductId=products[2].Id,
                    OrderDate = DateTime.UtcNow.AddHours(-2),
                    Status = "Delivered"
                }
            };
            await context.Orders.AddRangeAsync(orders);

            await context.SaveChangesAsync();
        }
    }
}
