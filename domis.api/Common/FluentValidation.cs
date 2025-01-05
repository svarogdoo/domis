using domis.api.DTOs.Product;
using domis.api.Models;
using FluentValidation;

namespace domis.api.Common;

public class CreateCartItemRequestValidator : AbstractValidator<CreateCartItemRequest>
{
    public CreateCartItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than zero.");

        RuleFor(x => x.Quantity)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
    }
}