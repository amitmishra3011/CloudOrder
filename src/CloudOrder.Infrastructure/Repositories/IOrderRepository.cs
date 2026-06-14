using CloudOrder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudOrder.Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync();
    }
}
