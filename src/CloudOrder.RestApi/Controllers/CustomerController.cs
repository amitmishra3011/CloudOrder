using CloudOrder.Business.DTOs.Customers;
using CloudOrder.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudOrder.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    [HttpGet]
    public async Task<IActionResult> Customers()
    {
        var customers = await _customerService.GetCustomersAsync();
        return Ok(customers);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Customer(Guid id)
    {
        var customer = await _customerService.GetCustomerAsync(id);
        return Ok(customer);
    }
    [HttpPost]
    public async Task<IActionResult> Customer([FromBody] CreateCustomerRequestDto request)
    {
        var created = await _customerService.CreateCustomerAsync(request);
        return CreatedAtAction(nameof(Customer), new { id = created.Id }, created);
    }
}
