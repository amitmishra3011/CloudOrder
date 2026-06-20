using CloudOrder.Domain.Entities;
using CloudOrder.Business.Repositories;

namespace CloudOrder.Application;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        return await _orderRepository.GetOrdersAsync();
    }
}
