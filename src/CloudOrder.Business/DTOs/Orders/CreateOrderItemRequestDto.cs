using System.ComponentModel.DataAnnotations;

namespace CloudOrder.Business.DTOs.Orders;

public class CreateOrderItemRequestDto
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
