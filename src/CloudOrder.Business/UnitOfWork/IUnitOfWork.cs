
using CloudOrder.Business.Repositories;

namespace CloudOrder.Business.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
   // IProductRepository Products { get; }
    Task<int> SaveChangesAsync();
}
