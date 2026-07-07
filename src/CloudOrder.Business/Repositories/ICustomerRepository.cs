using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<bool> CustomerExistsAsync(Guid customerId);

}
