using CloudOrder.Business.Repositories;
using CloudOrder.EFInfrastructure.Persistence;
using CloudOrder.EFInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudOrder.EFInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCloudOrderEFInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CloudOrderDbContext>(
            options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
