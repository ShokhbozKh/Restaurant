using Application.Extensions;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Restaurants.Api.Middlewares;
using Serilog;
using Restaurants.Api.Extensions;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();

// Swagger konfiguratsiyasi
/*
builder.Services.AddEndpointsApiExplorer();
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
            [] // bu yerda scopes bo‘ladi, lekin bizda yo‘q
        }
    });
});
*/
//  Muhim qo‘shimchalar — Identity API uchun zarur
builder.Services.AddDataProtection(); // IDataProtectionProvider uchun
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System); // TimeProvider uchun

// Infrastructure servislar
builder.Services.AddInfrastructureServices(builder.Configuration);

// Identity API (.NET 8 minimal identity)
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();
// 🔒 Identity uchun Bearer authentication qo‘shish
builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);
// 🧩 Identity SignIn, Token va Manager servislari
builder.Services.AddAuthorization();


// Application qatlam
builder.Services.AddApplicationServices();


var app = builder.Build();

// Migration + Seeder
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var restaurantSeeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

    dbContext.Database.Migrate();
    await restaurantSeeder.SeedAsync();
}
// Middleware lar
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeLoggingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// 🔐 Auth middleware
app.UseAuthentication();
app.UseAuthorization();

// Identity API minimal endpoints
app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapControllers();

app.Run();
