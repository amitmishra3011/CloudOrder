using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersAsync();

    Task<Order> GetOrderByIdAsync(Guid orderId);
}
