using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Mappings;

public static class OrderMappingExtensions
{
    public static OrderResponseDto ToDto(this Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            CustomerName = order.Customer.Name,
            OrderDate = order.CreatedDate,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
    public static List<OrderResponseDto> ToDto(this List<Order> orders)
    {
        return orders.Select(o => o.ToDto()).ToList();
    }
}
