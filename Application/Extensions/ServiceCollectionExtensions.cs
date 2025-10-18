using Application.AssignUsers;
using Application.Dishes;
using Application.Restaurants;
using Application.Users;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Application.AssignUsers;

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
        
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserDetailsService, UserDetailsService>();  
        services.AddScoped<IAssignUserService, AssignUserService>();
    }
}
