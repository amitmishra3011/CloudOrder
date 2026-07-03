using CloudOrder.Entities.Entities;
using System;
using System.Collections.Generic;

namespace CloudOrder.Business.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersAsync();

    Task<Order> GetOrderByIdAsync(Guid orderId);

    Task<Order> AddOrderAsync(Order order);

    Task<List<Product>> GetProductsByIdsAsync(IEnumerable<Guid> productIds);

    Task<bool> CustomerExistsAsync(Guid customerId);
}
