using CloudOrder.Business.DTOs.Orders;

namespace CloudOrder.Business.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetOrdersAsync();
        Task<OrderResponseDto> GetOrderAsync(Guid orderId);
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request);
    }
}
