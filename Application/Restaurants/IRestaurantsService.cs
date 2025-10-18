using Application.Common;
using Application.Restaurants.Dtos;
using Domain.Commands;
using Domain.Entities;

namespace Application.Restaurants;

public interface IRestaurantsService
{
    Task<IEnumerable<RestaurantDto>> GetAllAsync();
    Task<PagedResult<RestaurantDto>> GetPagedResultAsync(GetAllRestaurantsQuery query);
    Task<RestaurantDto?> GetByIdAsync(int id);
    Task<IEnumerable<RestaurantListDto>> GetListAsync();
    Task<int> CreateAsync(CreateRestaurantDto dto);
    Task UpdateAsync(int id, UpdateRestaurantDto dto);
    Task DeleteAsync(int id);

}
