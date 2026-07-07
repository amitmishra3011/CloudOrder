using AutoMapper;
using CloudOrder.Business.DTOs.Customers;
using CloudOrder.Business.Interfaces;
using CloudOrder.Business.Repositories;
using CloudOrder.Business.UnitOfWork;

namespace CloudOrder.Business.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CustomerService(IUnitOfWork  unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request)
    {
        var customer = _mapper.Map<Entities.Entities.Customer>(request);
        var created = await _unitOfWork.Customers.AddAsync(customer);
        _unitOfWork.SaveChangesAsync().Wait();
        return _mapper.Map<CustomerResponseDto>(created);
    }

    public async Task<CustomerResponseDto> GetCustomerAsync(Guid id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<List<CustomerResponseDto>> GetCustomersAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return _mapper.Map<List<CustomerResponseDto>>(customers);
    }
}
