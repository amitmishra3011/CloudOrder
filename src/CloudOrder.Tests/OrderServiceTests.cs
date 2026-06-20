using CloudOrder.Business;
using CloudOrder.Business.Repositories;
using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Entities.Entities;
using Moq;

namespace CloudOrder.Tests;

[TestClass]
public sealed class OrderServiceTests
{
    [TestMethod]
    public async Task GetOrdersAsync_ReturnsOrdersFromRepository()
    {
        List<Order> repoOrders = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                TotalAmount = 19.99m
            }
        };

        // Expected DTOs that the service should return after mapping
        List<OrderResponseDto> expectedDtos = repoOrders
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderDate = o.CreatedDate,
                TotalAmount = o.TotalAmount
            })
            .ToList();

        Mock<IOrderRepository> repository = new();
        // repository returns entity list (what the service will map)
        repository
            .Setup(repo => repo.GetOrdersAsync())
            .ReturnsAsync(repoOrders);

        OrderService service = new(repository.Object);

        List<OrderResponseDto> actualDtos = await service.GetOrdersAsync();

        // Compare by properties rather than reference equality
        Assert.AreEqual(expectedDtos.Count, actualDtos.Count);
        for (int i = 0; i < expectedDtos.Count; i++)
        {
            Assert.AreEqual(expectedDtos[i].Id, actualDtos[i].Id);
            Assert.AreEqual(expectedDtos[i].TotalAmount, actualDtos[i].TotalAmount);
            Assert.AreEqual(expectedDtos[i].OrderDate, actualDtos[i].OrderDate);
        }

        repository.Verify(repo => repo.GetOrdersAsync(), Times.Once);
    }
}
