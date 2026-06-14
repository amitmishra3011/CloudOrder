using CloudOrder.Domain.Entities;
using CloudOrder.Infrastructure.Repositories;

namespace CloudOrder.Application;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _orderRepository.GetOrdersAsync();
    }
}
