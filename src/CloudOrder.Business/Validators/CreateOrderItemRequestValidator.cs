using CloudOrder.Business.DTOs.Orders;
using FluentValidation;

namespace CloudOrder.Business.Validators;

public sealed class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemRequestValidator()
    {
        // Validate that the ProductId is not empty
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        // Validate that the Quantity is greater than 0
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .LessThanOrEqualTo(int.MaxValue)
            .WithMessage("Quantity must be greater than 0.");
    }
}
