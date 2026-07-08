using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudOrder.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudOrder.EFInfrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(CloudOrderDbContext context)
        {
            if (await context.Customers.AnyAsync()
                || await context.Products.AnyAsync()
                || await context.Orders.AnyAsync())
            {
                return;
            }

            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Acme Corp",
                    Email = "sales@acme.test",
                    Address = "Royal Serene A block, Pune"
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Contoso Ltd",
                    Email = "info@contoso.test",
                    Address = "Royal Serene B block, Pune"
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Fabrikam",
                    Email = "contact@fabrikam.test",
                    Address = "Kohinoor Sapphire-1 A block, Pune"
                }
            };

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Widget A", Price = 9.99m },
                new Product { Id = Guid.NewGuid(), Name = "Widget B", Price = 19.99m },
                new Product { Id = Guid.NewGuid(), Name = "Gadget", Price = 29.99m }
            };

            var orders = new List<Order>
            {
                CreateOrder(
                    customers[0],
                    new[]
                    {
                        (products[0], 1),
                        (products[1], 2)
                    },
                    DateTime.UtcNow),

                CreateOrder(
                    customers[1],
                    new[]
                    {
                        (products[1], 1),
                        (products[2], 1)
                    },
                    DateTime.UtcNow.AddMinutes(-30)),

                CreateOrder(
                    customers[2],
                    new[]
                    {
                        (products[0], 3),
                        (products[2], 2)
                    },
                    DateTime.UtcNow.AddHours(-2))
            };

            await context.Customers.AddRangeAsync(customers);
            await context.Products.AddRangeAsync(products);
            await context.Orders.AddRangeAsync(orders);

            await context.SaveChangesAsync();
        }

        private static Order CreateOrder(
            Customer customer,
            IEnumerable<(Product Product, int Quantity)> items,
            DateTime createdDate)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Customer = customer,
                CreatedDate = createdDate,
                Items = new List<OrderItem>()
            };

            customer.Orders.Add(order);

            decimal totalAmount = 0m;

            foreach (var (product, quantity) in items)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    Order = order,
                    Product = product,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };

                order.Items.Add(orderItem);
                product.OrderItems.Add(orderItem);

                totalAmount += orderItem.UnitPrice * orderItem.Quantity;
            }

            order.TotalAmount = totalAmount;

            return order;
        }
    }
}
