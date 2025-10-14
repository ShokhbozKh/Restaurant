using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Infrastructure.Seeders;
using Application.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Infrastructure qatlamdagi servislarni ro‘yxatdan o‘tkazish
//  json fayldan connection string ni o‘qib olish va db context ni ro‘yxatdan o‘tkazish
builder.Services.AddInfrastructureServices(builder.Configuration);

// Application qatlamdagi servislarni ro‘yxatdan o‘tkazish
builder.Services.AddApplicationServices();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// bu yerda db migration larni avtomatik qo‘llash va dastlabki ma'lumotlarni kiritish amalga oshiriladi
var scope = app.Services.CreateScope();
// bu yerda db migration larni avtomatik qo‘llash
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.SeedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
