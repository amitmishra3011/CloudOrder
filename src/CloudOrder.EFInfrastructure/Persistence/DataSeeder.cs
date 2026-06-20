using Microsoft.EntityFrameworkCore;
using CloudOrder.Entities.Entities;

namespace CloudOrder.EFInfrastructure.Persistence
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
                    Id = Guid.NewGuid(),
                    CustomerId = customers[0].Id,
                    CreatedDate = DateTime.UtcNow,
                    TotalAmount = products[0].Price,
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = products[0].Id,
                            Quantity = 1,
                            UnitPrice = products[0].Price
                        }
                    }
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customers[1].Id,
                    CreatedDate = DateTime.UtcNow.AddMinutes(-30),
                    TotalAmount = products[1].Price * 2,
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = products[1].Id,
                            Quantity = 2,
                            UnitPrice = products[1].Price
                        }
                    }
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customers[2].Id,
                    CreatedDate = DateTime.UtcNow.AddHours(-2),
                    TotalAmount = products[2].Price * 3,
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = products[2].Id,
                            Quantity = 3,
                            UnitPrice = products[2].Price
                        }
                    }
                }
            };
            await context.Orders.AddRangeAsync(orders);

            await context.SaveChangesAsync();
        }
    }
}
