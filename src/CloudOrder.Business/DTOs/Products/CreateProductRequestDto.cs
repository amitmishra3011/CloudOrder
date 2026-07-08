namespace CloudOrder.Business.DTOs.Products;

public class CreateProductRequestDto
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}
