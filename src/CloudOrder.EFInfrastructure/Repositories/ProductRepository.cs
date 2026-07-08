using CloudOrder.Business.Repositories;
using CloudOrder.EFInfrastructure.Persistence;
using CloudOrder.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudOrder.EFInfrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CloudOrderDbContext context) : base(context)
    {
    }

    public async override Task<List<Product>> GetAllAsync()
    {
        // include order items if desired; for now simple list
        return await base.GetAllAsync().ConfigureAwait(false);
    }

    public async override Task<Product> GetByIdAsync(Guid id)
    {
        // return product with related order items if needed
#pragma warning disable CS8603 // Possible null reference return.
        return await _dbContext.Products
            .Include(p => p.OrderItems)
            .ThenInclude(oi => oi.Order)
            .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async new Task<Product> AddAsync(Product entity)
    {
        return await base.AddAsync(entity).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Product entity)
    {
        await base.UpdateAsync(entity).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid id)
    {
        await base.DeleteAsync(id).ConfigureAwait(false);
    }
}
