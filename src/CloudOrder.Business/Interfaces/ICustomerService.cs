using CloudOrder.Business.DTOs.Customers;
using CloudOrder.Business.DTOs.Orders;

namespace CloudOrder.Business.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerResponseDto>> GetCustomersAsync();
    Task<CustomerResponseDto> GetCustomerAsync(Guid orderId);
    Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request);

    //Task UpdateCustomerAsync(Guid customerId, UpdateCustomerRequestDto request);
}
