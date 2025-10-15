using Application.Restaurants.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Restaurants.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    public CreateRestaurantDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .Length(3, 50);
        RuleFor(r => r.Category)
            .NotEmpty()
            .WithMessage("Category is required");
        RuleFor(r => r.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .When(r => !string.IsNullOrEmpty(r.PostalCode))
            .WithMessage("Postal code must be in the format XX-XXX");
        RuleFor(r => r.ContactEmail)
            .EmailAddress()
            .When(r => !string.IsNullOrEmpty(r.ContactEmail))
            .WithMessage("Invalid email address");
        RuleFor(r => r.ContactNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(r => !string.IsNullOrEmpty(r.ContactNumber))
            .WithMessage("Invalid phone number");

    }
}
