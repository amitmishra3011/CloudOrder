using System.ComponentModel.DataAnnotations;

namespace CloudOrder.Business.DTOs.Orders;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }

    public List<CreateOrderItemRequest> Items { get; set; }
        = new();
}
