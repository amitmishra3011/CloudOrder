using CloudOrder.Business.Services;
using CloudOrder.Entities.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NetArchTest.Rules;

namespace CloudOrder.Tests.ArchitectureTests;

[TestClass]
public class DependencyTests
{
    [TestMethod]
    public void Api_Should_Not_Depend_On_Persistence()
    {
        var result = Types
            .InAssembly(typeof(Program).Assembly)
            .ShouldNot()
            .HaveDependencyOn("CloudOrder.Persistence")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [TestMethod]
    public void Domain_Should_Not_Depend_On_EFCore()
    {
        var result = Types
            .InAssembly(typeof(Order).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Microsoft.EntityFrameworkCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    [TestMethod]
    public void Controllers_Should_Not_Depend_On_DbContext()
    {
        var result = Types
            .InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace("CloudOrder.Api.Controllers")
            .ShouldNot()
            .HaveDependencyOn("Microsoft.EntityFrameworkCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [TestMethod]
    public void Services_Should_Not_Depend_On_Repositories()
    {
        var result = Types
            .InAssembly(typeof(OrderService).Assembly)
            .That()
            .ResideInNamespace("CloudOrder.Application.Services")
            .ShouldNot()
            .HaveDependencyOn("CloudOrder.Persistence")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
