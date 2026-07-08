using CloudOrder.Entities.Entities;

namespace CloudOrder.Tests;

[TestClass]
public sealed class ProductServiceTests
{
    [TestMethod]
    public void Product_DefaultValues_AreInitialized()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.IsNotNull(product.OrderItems);
        Assert.AreEqual(0, product.OrderItems.Count);
        Assert.AreEqual(string.Empty, product.Name);
        Assert.AreEqual(0m, product.Price);
        Assert.AreEqual(Guid.Empty, product.Id);
    }

    [TestMethod]
    public void Product_AddOrderItem_BiDirectionalRelationship()
    {
        // Arrange
        var product = new Product { Id = Guid.NewGuid(), Name = "X", Price = 5m };
        var order = new Order { Id = Guid.NewGuid() };
        var orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            Order = order,
            Product = product,
            Quantity = 2,
            UnitPrice = product.Price
        };

        // Act
        product.OrderItems.Add(orderItem);

        // Assert
        Assert.AreEqual(1, product.OrderItems.Count);
        Assert.IsTrue(product.OrderItems.Contains(orderItem));
        Assert.AreSame(product, orderItem.Product);
    }
}
