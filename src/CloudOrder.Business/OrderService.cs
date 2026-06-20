using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Business.Mappings;
using CloudOrder.Business.Repositories;
using CloudOrder.Entities.Entities;

namespace CloudOrder.Business;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        var result = await _orderRepository.GetOrdersAsync();
        return result.ToDto();

    }

    public async Task<OrderResponseDto> GetOrderAysnc(Guid orderId)
    {
        var result = await _orderRepository.GetOrderByIdAsync(orderId);
        return result.ToDto();
    }
    public Task CreateOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }
}
