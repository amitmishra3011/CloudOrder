using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Entities.Entities;

namespace CloudOrder.Business
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetOrdersAsync();
        Task<OrderResponseDto> GetOrderAsync(Guid orderId);
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequest request);
    }
}
