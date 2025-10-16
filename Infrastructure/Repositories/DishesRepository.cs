using Domain.Dishes;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DishesRepository : IDishesRepository
{
    private readonly AppDbContext _context; // DI
    public DishesRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Dish>> GetAllDishesAsync(int restaurantId)
    {
        var dishes = await _context.Dishes
            .Where(x=>x.RestaurantId == restaurantId)
            .ToListAsync();
        return dishes;
    }

    public async Task<Dish?> GetDishByIdAsync(int restaurantId, int id)
    {
        var dish = await _context.Dishes
            .Where(x => x.RestaurantId == restaurantId)
            .FirstOrDefaultAsync(d => d.Id == id);
        return dish;
    }
    public  async Task<IEnumerable<Dish>> GetListDishesAsync(int restaurantId)
    {
        var list = await _context.Dishes
            .Where(x => x.RestaurantId == restaurantId)
            .ToListAsync();
        return list;
    }
    public async Task CreateDishAsync(Dish dish)
    {
        _context.Dishes.Add(dish);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDishAsync(Dish dish)
    {
        _context.Dishes.Update(dish);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteDishAsync(Dish dish)
    {
        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteDishesAsync(IEnumerable<Dish> dishes)
    {
        _context.Dishes.RemoveRange(dishes);
        await _context.SaveChangesAsync();
    }
}
