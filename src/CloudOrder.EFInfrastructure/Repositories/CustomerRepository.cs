using CloudOrder.Business.Repositories;
using CloudOrder.EFInfrastructure.Persistence;
using CloudOrder.Entities.Entities;

namespace CloudOrder.EFInfrastructure.Repositories;

public class CustomerRepository: Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(CloudOrderDbContext context) : base(context)
    {
        
    }
    public async Task<Customer> AddAsync(Customer entity)
    {
        return await base.AddAsync(entity).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid id)
    {
        await base.DeleteAsync(id).ConfigureAwait(false);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await base.GetAllAsync().ConfigureAwait(false);
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        return await base.GetByIdAsync(id).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Customer entity)
    {
        await base.UpdateAsync(entity);
    }
}
