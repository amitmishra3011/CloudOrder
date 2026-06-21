using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudOrder.Business;
using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Business.Repositories;
using CloudOrder.Entities.Entities;
using CloudOrder.Entities.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CloudOrder.Tests
{
    [TestClass]
    public sealed class OrderServiceTests
    {
        [TestMethod]
        public async Task GetOrdersAsync_ReturnsOrdersFromRepository()
        {
            var repoOrders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    TotalAmount = 19.99m
                }
            };

            var expectedDtos = repoOrders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderDate = o.CreatedDate,
                TotalAmount = o.TotalAmount
            }).ToList();

            var repository = new Mock<IOrderRepository>();
            repository.Setup(r => r.GetOrdersAsync()).ReturnsAsync(repoOrders);

            var service = new OrderService(repository.Object);

            var actual = await service.GetOrdersAsync();

            Assert.AreEqual(expectedDtos.Count, actual.Count);
            for (int i = 0; i < expectedDtos.Count; i++)
            {
                Assert.AreEqual(expectedDtos[i].Id, actual[i].Id);
                Assert.AreEqual(expectedDtos[i].TotalAmount, actual[i].TotalAmount);
                Assert.AreEqual(expectedDtos[i].OrderDate, actual[i].OrderDate);
            }

            repository.Verify(r => r.GetOrdersAsync(), Times.Once);
        }

        [TestMethod]
        public async Task CreateOrderAsync_Success_CreatesOrderAndReturnsDto()
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var request = new CreateOrderRequest
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemRequest>
                {
                    new CreateOrderItemRequest { ProductId = productId, Quantity = 2 }
                }
            };

            var product = new Product { Id = productId, Name = "P1", Price = 10m };
            var repository = new Mock<IOrderRepository>();
            repository
                .Setup(r => r.CustomerExistsAsync(customerId))
                .ReturnsAsync(true);
            repository
                .Setup(r => r.GetProductsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Product> { product });

            repository
                .Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            var service = new OrderService(repository.Object);

            var result = await service.CreateOrderAsync(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(20m, result.TotalAmount);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(product.Name, result.Items[0].ProductName);
            Assert.AreEqual(2, result.Items[0].Quantity);
            Assert.AreEqual(10m, result.Items[0].UnitPrice);

            repository.Verify(r => r.GetProductsByIdsAsync(It.IsAny<IEnumerable<Guid>>()), Times.Once);
            repository.Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Once);
        }

        [TestMethod]
        public async Task CreateOrderAsync_NullRequest_ThrowsBusinessException()
        {
            var repository = new Mock<IOrderRepository>();
            var service = new OrderService(repository.Object);

            try
            {
                await service.CreateOrderAsync(null!);
                Assert.Fail("Expected BusinessException was not thrown.");
            }
            catch (BusinessException)
            {
                // expected
            }
        }

        [TestMethod]
        public async Task CreateOrderAsync_MissingProduct_ThrowsBusinessException()
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var request = new CreateOrderRequest
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemRequest>
                {
                    new CreateOrderItemRequest { ProductId = productId, Quantity = 1 }
                }
            };

            var repository = new Mock<IOrderRepository>();
            repository
                .Setup(r => r.CustomerExistsAsync(customerId))
                .ReturnsAsync(true);
            repository
                .Setup(r => r.GetProductsByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(new List<Product>());

            var service = new OrderService(repository.Object);

            try
            {
                await service.CreateOrderAsync(request);
                Assert.Fail("Expected BusinessException was not thrown.");
            }
            catch (BusinessException)
            {
                // expected
            }
        }

        [TestMethod]
        public async Task CreateOrderAsync_EmptyItems_ThrowsBusinessException()
        {
            var customerId = Guid.NewGuid();

            var request = new CreateOrderRequest
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemRequest>()
            };

            var repository = new Mock<IOrderRepository>();
            repository.Setup(r => r.CustomerExistsAsync(customerId)).ReturnsAsync(true);

            var service = new OrderService(repository.Object);

            try
            {
                await service.CreateOrderAsync(request);
                Assert.Fail("Expected BusinessException was not thrown.");
            }
            catch (BusinessException)
            {
                // expected
            }
        }

        [TestMethod]
        public async Task CreateOrderAsync_NonPositiveQuantity_ThrowsBusinessException()
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var request = new CreateOrderRequest
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemRequest>
                {
                    new CreateOrderItemRequest { ProductId = productId, Quantity = 0 }
                }
            };

            var repository = new Mock<IOrderRepository>();
            repository.Setup(r => r.CustomerExistsAsync(customerId)).ReturnsAsync(true);

            var service = new OrderService(repository.Object);

            try
            {
                await service.CreateOrderAsync(request);
                Assert.Fail("Expected BusinessException was not thrown.");
            }
            catch (BusinessException)
            {
                // expected
            }
        }

        [TestMethod]
        public async Task CreateOrderAsync_CustomerNotFound_ThrowsNotFoundException()
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var request = new CreateOrderRequest
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemRequest>
                {
                    new CreateOrderItemRequest { ProductId = productId, Quantity = 1 }
                }
            };

            var repository = new Mock<IOrderRepository>();
            repository.Setup(r => r.CustomerExistsAsync(customerId)).ReturnsAsync(false);

            var service = new OrderService(repository.Object);

            try
            {
                await service.CreateOrderAsync(request);
                Assert.Fail("Expected NotFoundException was not thrown.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }
    }
}
