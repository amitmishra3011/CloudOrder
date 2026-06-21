using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Business.Mappings;
using CloudOrder.Business.Repositories;
using CloudOrder.Entities.Entities;
using CloudOrder.Entities.Exceptions;


namespace CloudOrder.Business;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        var result = await _orderRepository.GetOrdersAsync();
        return result.ToDto();

    }

    public async Task<OrderResponseDto> GetOrderAsync(Guid orderId)
    {
        var result = await _orderRepository.GetOrderByIdAsync(orderId);
        return result.ToDto();
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
            throw new NotFoundException($"Customer {request.CustomerId} not found.");

        if (request.Items == null || !request.Items.Any())
            throw new BusinessException("Order must contain at least one item.");

        if (request.Items.Any(i => i.Quantity <= 0))
            throw new BusinessException("All items must have quantity greater than zero.");

        // Fetch products to resolve unit prices
        var productIds = request.Items.Select(i => i.ProductId).Distinct();
        var products = await _orderRepository.GetProductsByIdsAsync(productIds);

        var foundIds = products.Select(p => p.Id).ToHashSet();
        var missing = productIds.Where(id => !foundIds.Contains(id)).ToList();
        if (missing.Any())
            throw new BusinessException($"Products not found: {string.Join(',', missing)}");

        // Build order entity
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CreatedDate = DateTime.UtcNow,
            Items = request.Items.Select(i =>
            {
                var product = products.First(p => p.Id == i.ProductId);
                return new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = product.Price,
                    Product = product
                };
            }).ToList()
        };

        order.TotalAmount = order.Items.Sum(x => x.UnitPrice * x.Quantity);

        var saved = await _orderRepository.AddOrderAsync(order);

        return saved.ToDto();
    }
}
