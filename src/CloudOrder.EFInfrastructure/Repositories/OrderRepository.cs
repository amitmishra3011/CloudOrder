using CloudOrder.Business.Repositories;
using CloudOrder.EFInfrastructure.Persistence;
using CloudOrder.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudOrder.EFInfrastructure.Repositories
{
    public class OrderRepository: Repository<Order>, IOrderRepository
    {
        public OrderRepository(CloudOrderDbContext context) : base(context)
        {
        }

        public async override Task<List<Order>> GetAllAsync()
        {
            return await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async override Task<Order> GetByIdAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id).ConfigureAwait(false);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async new Task<Order> AddAsync(Order entity)
        {
            return await base.AddAsync(entity);
        }

        public async Task<List<Product>> GetProductsByIdsAsync(
        IEnumerable<Guid> productIds)
        {
            return await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task UpdateAsync(Order entity)
        {
            await base.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }
    }
}
