using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Business.Repositories;
using CloudOrder.EFInfrastructure.Persistence;
using CloudOrder.Entities.Entities;
using CloudOrder.Entities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CloudOrder.EFInfrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CloudOrderDbContext _context;
        public OrderRepository(CloudOrderDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            // C#
            return (await _context.Orders.FindAsync(orderId))
                   ?? throw new NotFoundException($"Order {orderId} not found.");// C#
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> productIds)
        {
            return await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<bool> CustomerExistsAsync(Guid customerId)
        {
            return await _context.Customers.AnyAsync(c => c.Id == customerId);
        }
    }
}
