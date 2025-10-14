using Application.Restaurants;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Add application services
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}
