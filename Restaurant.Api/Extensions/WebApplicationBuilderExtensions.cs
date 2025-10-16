using Infrastructure.Persistence;
using Infrastructure.Seeders;
using Microsoft.OpenApi.Models;
using Restaurants.Api.Middlewares;
using Serilog;

namespace Restaurants.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme // bu swaggerda lock ikonka paydo bo‘lishi uchun
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement // bu swaggerda lock ikonka paydo bo‘lishi uchun
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    new string[]{} // bu yerda scopes bo‘ladi, lekin bizda yo‘q
                }
             });
        });

        builder.Services.AddEndpointsApiExplorer();

        // Middlewares
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

        // Serilog konfiguratsiyasi
        builder.Host.UseSerilog((context, conf) =>
                    conf.ReadFrom.Configuration(context.Configuration));

    }

}
