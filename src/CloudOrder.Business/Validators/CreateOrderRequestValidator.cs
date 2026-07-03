using CloudOrder.Business.DTOs.Orders;
using FluentValidation;

namespace CloudOrder.Business.Validators;

public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequestDto>
{
    public CreateOrderRequestValidator()
    {
        // Validate that the CustomerId is not empty
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId is required.");

        // Validate that the Items collection is not empty
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must contain at least one item.");

        // Validate each item in the Items collection using the CreateOrderItemRequestValidator
        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithMessage("ProductId is required.");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.");
            });
    }
}
