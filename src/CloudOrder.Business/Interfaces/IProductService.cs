using CloudOrder.Business.DTOs.Products;

namespace CloudOrder.Business.Interfaces;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetProductsAsync();
    Task<ProductResponseDto> GetProductAsync(Guid productId);
    Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request);
}
