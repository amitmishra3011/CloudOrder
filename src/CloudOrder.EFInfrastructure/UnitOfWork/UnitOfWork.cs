using CloudOrder.Business.Repositories;
using CloudOrder.EFInfrastructure.Persistence;
using CloudOrder.EFInfrastructure.Repositories;

namespace CloudOrder.Business.UnitOfWork;

public class UnitOfWork(CloudOrderDbContext context) : IUnitOfWork
{
    private readonly CloudOrderDbContext _context = context;
    private ICustomerRepository? _customers;
    private IOrderRepository? _orders;

    public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);
    public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
    // public IProductRepository Products => _products ??= new ProductRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
