using Application.Dishes.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dishes;

public interface IDishesService
{
    Task<IEnumerable<DishDto>> GetAllAsync(int restaurantId);
    Task<DishDto?> GetByIdAsync(int restaurantId, int DishId);
    Task<IEnumerable<DishListDto>> GetListAsync(int restaurantId);
    Task<int> CreateAsync(int restaurantId, CreateDishDto dto);
    Task UpdateAsync(int restaurantId ,int Dishid, UpdateDishDto dto);
    Task DeleteAsync(int restaurantId, int id);
    Task DeleteDishesForRestaurantAsync(int restaurantId);
}
