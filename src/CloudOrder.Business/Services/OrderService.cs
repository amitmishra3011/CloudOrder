using AutoMapper;
using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Business.Interfaces;
using CloudOrder.Business.UnitOfWork;
using CloudOrder.Entities.Entities;
using CloudOrder.Entities.Exceptions;


namespace CloudOrder.Business.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        var result = await _unitOfWork.Orders.GetAllAsync();
        return _mapper.Map<List<OrderResponseDto>>(result);
    }

    public async Task<OrderResponseDto> GetOrderAsync(Guid orderId)
    {
        var result = await _unitOfWork.Orders.GetByIdAsync(orderId);
        // how to use orderprofile for mapping here
        return _mapper.Map<OrderResponseDto>(result);
    }

    public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request)
    {
        // Verify customer exists
        var customerExists = await _unitOfWork.Customers.CustomerExistsAsync(request.CustomerId);
        if (!customerExists)
            throw new NotFoundException(
                $"Customer {request.CustomerId} does not exist.");

        // Fetch products to resolve unit prices
        var productIds = request.Items.Select(i => i.ProductId).Distinct();
        var products = await _unitOfWork.Orders.GetProductsByIdsAsync(productIds);
        var productLookup = products.ToDictionary(p => p.Id);
        var foundIds = products.Select(p => p.Id).ToHashSet();
        var missing = productIds.Where(id => !foundIds.Contains(id)).ToList();
        if (missing.Count() == 0)
            throw new BusinessException($"Products not found: {string.Join(',', missing)}");

        var order = _mapper.Map<Order>(request);

        foreach (var item in order.Items)
        {
            var product = productLookup[item.ProductId];

            item.UnitPrice = product.Price;
            item.Product = product;
        }

        order.TotalAmount = order.Items.Sum(x => x.UnitPrice * x.Quantity);

        var saved = await _unitOfWork.Orders.AddAsync(order);
        _unitOfWork.SaveChangesAsync().Wait();
        return _mapper.Map<OrderResponseDto>(saved);
    }
}
