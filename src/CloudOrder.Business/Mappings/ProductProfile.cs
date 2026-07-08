using AutoMapper;
using CloudOrder.Business.DTOs.Products;
using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Mappings;

internal class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductRequestDto, Product>();
        CreateMap<Product, ProductResponseDto>();
    }
}
