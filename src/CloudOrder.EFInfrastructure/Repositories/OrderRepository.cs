using CloudOrder.Business.Repositories;
using CloudOrder.Entities.Entities;
using CloudOrder.Entities.Exceptions;
using CloudOrder.EFInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CloudOrder.Business.DTOs.Orders;

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
    }
}
