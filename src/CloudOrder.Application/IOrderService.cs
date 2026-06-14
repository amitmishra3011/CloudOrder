using CloudOrder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudOrder.Application
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
    }
}
