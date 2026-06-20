using System;
using System.Collections.Generic;
using System.Text;

namespace CloudOrder.Business.DTOs.Orders;

public class OrderResponseDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemResponseDto> Items { get; set; }
        = new();
}
