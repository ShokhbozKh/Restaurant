using Bogus;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders;

public class RestaurantSeeder: IRestaurantSeeder
{
    private readonly AppDbContext _context;
    public RestaurantSeeder(AppDbContext context)
    {
        _context = context;
    }
    public async Task SeedAsync()
    {
        await SeederRestaurantAsync();
        await SeederDishAsync();

        if(! _context.Roles.Any())
        {
            var roles = GetRoles();
            await _context.Roles.AddRangeAsync(roles);
            await _context.SaveChangesAsync();
        }
    }
    private IEnumerable<IdentityRole> GetRoles()
    {
        var roles = new List<IdentityRole>
        {
            new IdentityRole(UserRoles.User)
            {
                NormalizedName = UserRoles.User.ToUpper()
            },
            new IdentityRole(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper()
            },
            new IdentityRole(UserRoles.Manager)
            {
                NormalizedName = UserRoles.User.ToUpper()
            },
            new IdentityRole(UserRoles.Owner)
            {
                NormalizedName = UserRoles.Owner.ToUpper()
            }

        };
        return roles;
    }
             
          
    private async Task SeederRestaurantAsync()
    {
        var userIds = _context.Users.Select(u => u.Id).ToList();

        if (!_context.Restaurants.Any())
        {
            Faker<Restaurant> faker = new Faker<Restaurant>()
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Description, f => f.Lorem.Sentence(5))
                .RuleFor(r => r.Category, f => f.PickRandom(new[] { "Fast Food", "Casual Dining", "Fine Dining", "Cafe", "Buffet" }))
                .RuleFor(r => r.HasDelivery, f => f.Random.Bool(0.7f))
                .RuleFor(r => r.ContactEmail, f => f.Internet.Email())
                .RuleFor(r => r.ContactNumber, f => f.Phone.PhoneNumber())
                .RuleFor(r => r.OwnerId, f => f.PickRandom(userIds))
                .RuleFor(r => r.Address, f => new Address
                {
                    City = f.Address.City(),
                    Street = f.Address.StreetAddress(),
                    PostalCode = f.Address.ZipCode()
                });
            
            var restaurants = faker.Generate(110);
            await _context.Restaurants.AddRangeAsync(restaurants);
            await _context.SaveChangesAsync();
        }
    }
    private async Task SeederDishAsync()
    {
        if (!_context.Dishes.Any())
        {
            var restaurantIds = _context.Restaurants.Select(r => r.Id).ToList();
            Faker<Dish> dishFaker = new Faker<Dish>()
                .RuleFor(d => d.Name, f => f.Commerce.ProductName())
                .RuleFor(d => d.Description, f => f.Lorem.Sentence(5))
                .RuleFor(d => d.Price, f => decimal.Parse(f.Commerce.Price(5, 100)))
                .RuleFor(d => d.RestaurantId, f => f.PickRandom(restaurantIds));
            var dishes = dishFaker.Generate(115);
            await _context.Dishes.AddRangeAsync(dishes);
            await _context.SaveChangesAsync();
        }

    }
}
