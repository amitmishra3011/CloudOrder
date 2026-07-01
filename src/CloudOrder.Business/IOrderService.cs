using CloudOrder.Business.DTOs.Orders;

namespace CloudOrder.Business
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetOrdersAsync();
        Task<OrderResponseDto> GetOrderAsync(Guid orderId);
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequest request);
    }
}
