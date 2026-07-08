using AutoMapper;
using CloudOrder.Business.DTOs.Customers;
using CloudOrder.Business.Mappings;
using CloudOrder.Business.Services;
using CloudOrder.Business.UnitOfWork;
using CloudOrder.Entities.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CloudOrder.Tests;

[TestClass]
public sealed class CustomerServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private IMapper _mapper = null!;
    private CustomerService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CustomerProfile>();
        }, NullLoggerFactory.Instance);

        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();

        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _service = new CustomerService(
            _unitOfWorkMock.Object,
            _mapper);
    }

    [TestMethod]
    public async Task GetCustomersAsync_ReturnsMappedCustomers()
    {
        // Arrange
        var customers = new List<Customer>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Alice",
                    Email = "alice@test",
                    Address = "Somewhere"
                }
            };

        _unitOfWorkMock
            .Setup(u => u.Customers.GetAllAsync())
            .ReturnsAsync(customers);

        // Act
        var result = await _service.GetCustomersAsync();

        // Assert
        Assert.HasCount(1, result);
        Assert.AreEqual("Alice", result[0].Name);
        Assert.AreEqual("alice@test", result[0].Email);

        _unitOfWorkMock.Verify(u => u.Customers.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetCustomerAsync_ReturnsMappedCustomer()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer
        {
            Id = id,
            Name = "Bob",
            Email = "bob@test",
            Address = "Here"
        };

        _unitOfWorkMock
            .Setup(u => u.Customers.GetByIdAsync(id))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetCustomerAsync(id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(id, result.Id);
        Assert.AreEqual("Bob", result.Name);

        _unitOfWorkMock.Verify(u => u.Customers.GetByIdAsync(id), Times.Once);
    }

    [TestMethod]
    public async Task CreateCustomerAsync_CreatesAndReturnsMappedCustomer()
    {
        // Arrange
        var request = new CreateCustomerRequestDto
        {
            Name = "NewCo",
            Email = "newco@test",
            Address = "123 Lane"
        };

        _unitOfWorkMock
            .Setup(u => u.Customers.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync((Customer c) => c);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateCustomerAsync(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("NewCo", result.Name);
        Assert.AreEqual("newco@test", result.Email);

        _unitOfWorkMock.Verify(u => u.Customers.AddAsync(It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
