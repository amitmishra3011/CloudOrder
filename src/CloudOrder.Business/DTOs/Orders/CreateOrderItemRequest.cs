using System.ComponentModel.DataAnnotations;

namespace CloudOrder.Business.DTOs.Orders;

public class CreateOrderItemRequest
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
