using AutoMapper;
using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Business.Interfaces;
using CloudOrder.Business.Repositories;
using CloudOrder.Entities.Entities;
using CloudOrder.Entities.Exceptions;


namespace CloudOrder.Business.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        var result = await _orderRepository.GetOrdersAsync();
        return _mapper.Map<List<OrderResponseDto>>(result);
    }

    public async Task<OrderResponseDto> GetOrderAsync(Guid orderId)
    {
        var result = await _orderRepository.GetOrderByIdAsync(orderId);
        // how to use orderprofile for mapping here
        return _mapper.Map<OrderResponseDto>(result);
    }

    public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequest request)
    {
        if (request is null)
            throw new BusinessException("Request cannot be null.");

        if (request.CustomerId == Guid.Empty)
            throw new BusinessException("CustomerId is required.");

        // Verify customer exists
        var customerExists = await _orderRepository.CustomerExistsAsync(request.CustomerId);
        if (!customerExists)
            throw new NotFoundException(
                $"Customer {request.CustomerId} does not exist.");

        if (request.Items == null || !request.Items.Any())
            throw new BusinessException("Order must contain at least one item.");

        if (request.Items.Any(i => i.Quantity <= 0))
            throw new BusinessException("All items must have quantity greater than zero.");

        // Fetch products to resolve unit prices
        var productIds = request.Items.Select(i => i.ProductId).Distinct();
        var products = await _orderRepository.GetProductsByIdsAsync(productIds);
        var productLookup = products.ToDictionary(p => p.Id);
        var foundIds = products.Select(p => p.Id).ToHashSet();
        var missing = productIds.Where(id => !foundIds.Contains(id)).ToList();
        if (missing.Any())
            throw new BusinessException($"Products not found: {string.Join(',', missing)}");

        var order = _mapper.Map<Order>(request);

        foreach (var item in order.Items)
        {
            var product = productLookup[item.ProductId];

            item.UnitPrice = product.Price;
            item.Product = product;
        }

        order.TotalAmount = order.Items.Sum(x => x.UnitPrice * x.Quantity);

        var saved = await _orderRepository.AddOrderAsync(order);

        return _mapper.Map<OrderResponseDto>(saved);
    }
}
