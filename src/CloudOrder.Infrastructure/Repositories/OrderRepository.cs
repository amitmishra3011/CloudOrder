using CloudOrder.Domain.Entities;
using CloudOrder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CloudOrder.Infrastructure.Repositories
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
    }
}
