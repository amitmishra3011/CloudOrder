using System.ComponentModel.DataAnnotations;

namespace CloudOrder.Business.DTOs.Orders;

public class CreateOrderRequest
{
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<CreateOrderItemRequest> Items { get; set; }
        = new();
}
