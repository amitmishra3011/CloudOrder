using CloudOrder.Business;
using CloudOrder.Business.Repositories;
using CloudOrder.Entities.Entities;
using Moq;

namespace CloudOrder.Tests;

[TestClass]
public sealed class OrderServiceTests
{
    [TestMethod]
    public async Task GetOrdersAsync_ReturnsOrdersFromRepository()
    {
        var expectedOrders = new List<Order>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                TotalAmount = 19.99m
            }
        };

        var repository = new Mock<IOrderRepository>();
        repository
            .Setup(repo => repo.GetOrdersAsync())
            .ReturnsAsync(expectedOrders);

        var service = new OrderService(repository.Object);

        var actualOrders = await service.GetOrdersAsync();

        Assert.AreEqual(expectedOrders.Count, actualOrders.Count);
        Assert.AreEqual(expectedOrders[0].Id, actualOrders[0].Id);
        Assert.AreEqual(expectedOrders[0].TotalAmount, actualOrders[0].TotalAmount);
        repository.Verify(repo => repo.GetOrdersAsync(), Times.Once);
    }
}
