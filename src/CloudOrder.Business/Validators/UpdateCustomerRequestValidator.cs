using CloudOrder.Business.DTOs.Customers;
using FluentValidation;

namespace CloudOrder.Business.Validators;

public sealed class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequestDto>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .MaximumLength(200)
            .WithMessage("Address cannot exceed 200 characters.");
    }
}

