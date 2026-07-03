using CloudOrder.Business.Validators;
using FluentValidation;

namespace CloudOrder.RestApi.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidationExtensions(this IServiceCollection services)
    {
        // Register all validators from the Business assembly
        services.AddValidatorsFromAssemblyContaining<CreateOrderItemRequestValidator>();

        // If your FluentValidation version exposes AddFluentValidationAutoValidation(),
        // you can also enable automatic model validation here:
        // services.AddFluentValidationAutoValidation();

        return services;
    }
}
