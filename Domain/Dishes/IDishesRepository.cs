using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dishes;

public interface IDishesRepository
{
    Task<IEnumerable<Dish>> GetAllDishesAsync(int restaurantId);
    Task<IEnumerable<Dish>> GetListDishesAsync(int restaurantId);
    Task<Dish?> GetDishByIdAsync(int restaurantId, int id);
    Task CreateDishAsync(Dish dish);
    Task UpdateDishAsync(Dish dish);
    Task DeleteDishAsync(Dish dish);
    Task DeleteDishesAsync(IEnumerable<Dish> dishes);
}
