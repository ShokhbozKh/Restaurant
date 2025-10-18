using Domain.Commands;
using Domain.Entities;

namespace Domain.Restaurants;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<(IEnumerable<Restaurant>,int)> GetPagedAsync(GetAllRestaurantsQuery query);
    Task<Restaurant?> GetByIdAsync(int id);
    Task<IEnumerable<Restaurant>> GetListAsync();
    Task CreateAsync(Restaurant restaurant);
    Task UpdateAsync(Restaurant restaurant);
    Task DeleteAsync(Restaurant restaurant);
    Task<bool> ExistsAsync(int restaurantId);
}
