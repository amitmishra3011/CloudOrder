namespace CloudOrder.Business.DTOs.Orders;

public class OrderResponseDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<OrderItemResponseDto> Items { get; set; }
        = new();
}
