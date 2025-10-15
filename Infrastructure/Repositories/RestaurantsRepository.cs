using Domain.Entities;
using Domain.Restaurants;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RestaurantsRepository : IRestaurantsRepository
{
    private readonly AppDbContext _context; // DI
    public RestaurantsRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var result = await _context.Restaurants
            .Include(d=> d.Dishes) 
            .ToListAsync();
        return result;
    }
    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var result = await _context.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
        return result;
    }
    public async Task<IEnumerable<Restaurant>> GetListAsync()
    {
        var list = await _context.Restaurants.ToListAsync();
        return list;
    }
    public async Task CreateAsync(Restaurant restaurant)
    {
        await _context.Restaurants.AddAsync(restaurant);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Restaurant restaurant)
    {
        _context.Restaurants.Update(restaurant);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Restaurant restaurant)
    {
        _context.Restaurants.Remove(restaurant);
        await _context.SaveChangesAsync();
    }
}
