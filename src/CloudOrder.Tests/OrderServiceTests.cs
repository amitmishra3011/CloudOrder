using AutoMapper;
using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Business.DTOs.Orders.Mappings;
using CloudOrder.Business.Repositories;
using CloudOrder.Business.Services;
using CloudOrder.Entities.Entities;
using CloudOrder.Entities.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace CloudOrder.Tests
{
    [TestClass]
    public sealed class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock = null!;
        private IMapper _mapper = null!;
        private OrderService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderProfile>();
            }, NullLoggerFactory.Instance);

            config.AssertConfigurationIsValid();

            _mapper = config.CreateMapper();

            _orderRepositoryMock = new Mock<IOrderRepository>();

            _service = new OrderService(
                _orderRepositoryMock.Object,
                _mapper);
        }

        [TestMethod]
        public async Task GetOrdersAsync_ReturnsMappedOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    TotalAmount = 100,
                    Customer = new Customer
                    {
                        Name = "John"
                    },
                    Items = new List<OrderItem>()
                }
            };

            _orderRepositoryMock
                .Setup(r => r.GetOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _service.GetOrdersAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("John", result[0].CustomerName);
            Assert.AreEqual(100m, result[0].TotalAmount);

            _orderRepositoryMock.Verify(
                r => r.GetOrdersAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task GetOrderAsync_ReturnsMappedOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.UtcNow,
                TotalAmount = 50,
                Customer = new Customer
                {
                    Name = "Mac"
                },
                Items = new List<OrderItem>()
            };

            _orderRepositoryMock
                .Setup(r => r.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _service.GetOrderAsync(orderId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(orderId, result.Id);
            Assert.AreEqual("Mac", result.CustomerName);

            _orderRepositoryMock.Verify(
                r => r.GetOrderByIdAsync(orderId),
                Times.Once);
        }
        [TestMethod]
        public async Task CreateOrderAsync_NullRequest_ThrowsBusinessException()
        {
            await Assert.ThrowsAsync<BusinessException>(
                () => _service.CreateOrderAsync(null!));
        }

        [TestMethod]
        public async Task CreateOrderAsync_EmptyCustomerId_ThrowsBusinessException()
        {
            var request = new CreateOrderRequest
            {
                CustomerId = Guid.Empty,
                Items = new List<CreateOrderItemRequest>()
            };

            await Assert.ThrowsAsync<BusinessException>(
                () => _service.CreateOrderAsync(request));
        }
        [TestMethod]
        public async Task CreateOrderAsync_CustomerNotFound_ThrowsNotFoundException()
        {
            var request = new CreateOrderRequest
            {
                CustomerId = Guid.NewGuid(),
                Items =
                [
                    new()
            {
                ProductId = Guid.NewGuid(),
                Quantity = 1
            }
                ]
            };

            _orderRepositoryMock
                .Setup(r => r.CustomerExistsAsync(request.CustomerId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateOrderAsync(request));
        }
        [TestMethod]
        public async Task CreateOrderAsync_NoItems_ThrowsBusinessException()
        {
            var request = new CreateOrderRequest
            {
                CustomerId = Guid.NewGuid(),
                Items = []
            };

            _orderRepositoryMock
                .Setup(r => r.CustomerExistsAsync(request.CustomerId))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessException>(
                () => _service.CreateOrderAsync(request));
        }
        [TestMethod]
        public async Task CreateOrderAsync_InvalidQuantity_ThrowsBusinessException()
        {
            var request = new CreateOrderRequest
            {
                CustomerId = Guid.NewGuid(),
                Items =
                [
                    new()
            {
                ProductId = Guid.NewGuid(),
                Quantity = 0
            }
                ]
            };

            _orderRepositoryMock
                .Setup(r => r.CustomerExistsAsync(request.CustomerId))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessException>(
                () => _service.CreateOrderAsync(request));
        }
        [TestMethod]
        public async Task CreateOrderAsync_ProductNotFound_ThrowsBusinessException()
        {
            var productId = Guid.NewGuid();

            var request = new CreateOrderRequest
            {
                CustomerId = Guid.NewGuid(),
                Items =
                [
                    new()
            {
                ProductId = productId,
                Quantity = 1
            }
                ]
            };

            _orderRepositoryMock
                .Setup(r => r.CustomerExistsAsync(request.CustomerId))
                .ReturnsAsync(true);

            _orderRepositoryMock
                .Setup(r => r.GetProductsByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync([]);

            await Assert.ThrowsAsync<BusinessException>(
                () => _service.CreateOrderAsync(request));
        }

        [TestMethod]
        public async Task CreateOrderAsync_Success_ReturnsCreatedOrder()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var request = new CreateOrderRequest
            {
                CustomerId = customerId,
                Items =
                [
                    new()
            {
                ProductId = productId,
                Quantity = 2
            }
                ]
            };

            var product = new Product
            {
                Id = productId,
                Name = "Laptop",
                Price = 10m
            };

            _orderRepositoryMock
                .Setup(r => r.CustomerExistsAsync(customerId))
                .ReturnsAsync(true);

            _orderRepositoryMock
                .Setup(r => r.GetProductsByIdsAsync(
                    It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync([product]);

            _orderRepositoryMock
                .Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            // Act
            var result =
                await _service.CreateOrderAsync(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(20m, result.TotalAmount);

            Assert.AreEqual(
                "Laptop",
                result.Items[0].ProductName);

            Assert.AreEqual(
                10m,
                result.Items[0].UnitPrice);

            Assert.AreEqual(
                2,
                result.Items[0].Quantity);

            _orderRepositoryMock.Verify(
                r => r.AddOrderAsync(It.IsAny<Order>()),
                Times.Once);
        }
    }
}
