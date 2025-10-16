using Application.Dishes.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dishes.Validators;

public class CreateDishDtoValidator: AbstractValidator<CreateDishDto>
{
    public CreateDishDtoValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(30);
        RuleFor(d => d.Description)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(d => d.Price)
            .GreaterThan(0)
            .WithMessage("0 dan katta bo'lishi kerak");
        RuleFor(d => d.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("0 dan katta bo'lishi kerak")
            .When(d => d.KiloCalories.HasValue);
        RuleFor(d => d.RestaurantId)
            .GreaterThan(0);

    }
}
