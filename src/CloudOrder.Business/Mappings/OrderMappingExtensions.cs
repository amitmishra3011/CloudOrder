using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Mappings;

public static class OrderMappingExtensions
{
    public static OrderResponseDto ToDto(this Order order)
    {
        if (order is null)
            return new OrderResponseDto();

        return new OrderResponseDto
        {
            Id = order.Id,
            CustomerName = order.Customer?.Name ?? string.Empty,
            OrderDate = order.CreatedDate,
            TotalAmount = order.TotalAmount,
            Items = (order.Items ?? new List<OrderItem>())
                .Select(i => new OrderItemResponseDto
                {
                    ProductName = i.Product?.Name ?? string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
        };
    }
    public static List<OrderResponseDto> ToDto(this List<Order> orders)
    {
        if (orders is null)
            return new List<OrderResponseDto>();

        return orders.Select(o => o.ToDto()).ToList();
    }
}
