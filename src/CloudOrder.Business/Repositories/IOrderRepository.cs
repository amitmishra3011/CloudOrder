using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Product>> GetProductsByIdsAsync(
       IEnumerable<Guid> productIds);
}
