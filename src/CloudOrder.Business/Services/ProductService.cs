using AutoMapper;
using CloudOrder.Business.DTOs.Products;
using CloudOrder.Business.Interfaces;
using CloudOrder.Business.UnitOfWork;

namespace CloudOrder.Business.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request)
    {
        var product = _mapper.Map<Entities.Entities.Product>(request);
        var created = await _unitOfWork.Products.AddAsync(product);
        _unitOfWork.SaveChangesAsync().Wait();
        return _mapper.Map<ProductResponseDto>(created);
    }

    public async Task<ProductResponseDto> GetProductAsync(Guid productId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<List<ProductResponseDto>> GetProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return _mapper.Map<List<ProductResponseDto>>(products);
    }
}
