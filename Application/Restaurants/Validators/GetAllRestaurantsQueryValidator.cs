using Application.Restaurants.Dtos;
using Domain.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Restaurants.Validators;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowPageSize = [5,10,20,50, 100];
    private string[] allowSortBy = [nameof(RestaurantDto.Name), nameof(RestaurantDto.Description), nameof(RestaurantDto.Category)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThan(0);
        RuleFor(r=>r.PageSize)
            .Must(ps=> allowPageSize.Contains(ps)) // tekshiradi
            .WithMessage($"Page size must be one of the following values: {string.Join(", ", allowPageSize)}");

        RuleFor(r=>r.SortBy)
            .Must(value=> allowSortBy.Contains(value))
            .When(q=>q.SortBy !=null)
            .WithMessage($"SortBy must be one of the following values: {string.Join(", ", allowSortBy)}");
    }
}
