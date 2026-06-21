using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Entities.Entities;

namespace CloudOrder.Business
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetOrdersAsync();
        Task<OrderResponseDto> GetOrderAysnc(Guid orderId);
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequest request);
    }
}
