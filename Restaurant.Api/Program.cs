using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Infrastructure.Seeders;
using Application.Extensions;
using Serilog;
using Serilog.Events;
using Restaurants.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Global error handling middleware ni ro‘yxatdan o‘tkazish
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

// Infrastructure qatlamdagi servislarni ro‘yxatdan o‘tkazish
//  json fayldan connection string ni o‘qib olish va db context ni ro‘yxatdan o‘tkazish
builder.Services.AddInfrastructureServices(builder.Configuration);

// Application qatlamdagi servislarni ro‘yxatdan o‘tkazish
builder.Services.AddApplicationServices();

// swagger ni sozlash
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog ni sozlash
builder.Host.UseSerilog((context, conf) =>
    conf.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
// bu yerda db migration larni avtomatik qo‘llash va dastlabki ma'lumotlarni kiritish amalga oshiriladi
using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var restaurantSeeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
    // migration larni qo‘llash
    dbContext.Database.Migrate();
    // dastlabki ma'lumotlarni kiritish
    await restaurantSeeder.SeedAsync();

}

// bu yerda global error handling middleware ni qo‘shish
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeLoggingMiddleware>();

// bu yerda http so‘rovlarni log qilish uchun middleware ni qo‘shish
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
