using Application.Restaurants;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Application.Dishes;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var appAssembly = typeof(ServiceCollectionExtensions).Assembly;
        // Add application services
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddScoped<IDishesService, DishesService>();

        // Add AutoMapper and FluentValidation
        services.AddAutoMapper(appAssembly);
        services.AddValidatorsFromAssembly(appAssembly)
            .AddFluentValidationAutoValidation();// bu validationni avtomatik ishlatadi
    }
}
