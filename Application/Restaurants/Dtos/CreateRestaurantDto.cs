using Application.Dishes.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Restaurants.Dtos;

public class CreateRestaurantDto
{
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    [Required(ErrorMessage ="Category is required")]
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }

    public string? City { get; set; }
    public string? Street { get; set; }
    [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Postal code must be in the format XX-XXX")]
    public string? PostalCode { get; set; }
    [EmailAddress (ErrorMessage ="Email address")]
    public string? ContactEmail { get; set; }
    [Phone(ErrorMessage ="Phone number")]
    public string? ContactNumber { get; set; }

}
