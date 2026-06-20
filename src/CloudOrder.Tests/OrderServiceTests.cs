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
        List<Order> expectedOrders = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                TotalAmount = 19.99m
            }
        };

        Mock<IOrderRepository> repository = new();
        repository
            .Setup(repo => repo.GetOrdersAsync())
            .ReturnsAsync(expectedOrders);

        OrderService service = new(repository.Object);

        List<Order> actualOrders = await service.GetOrdersAsync();

        CollectionAssert.AreEqual(expectedOrders, actualOrders);
        repository.Verify(repo => repo.GetOrdersAsync(), Times.Once);
    }
}
