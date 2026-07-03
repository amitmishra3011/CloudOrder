namespace CloudOrder.Business.DTOs.Customers;

public class CreateCustomerRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}
