using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<bool> CustomerExistsAsync(Guid customerId);
    Task<List<Product>> GetProductsByIdsAsync(
       IEnumerable<Guid> productIds);
}
