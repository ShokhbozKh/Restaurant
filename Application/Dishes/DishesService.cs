using Application.Dishes.Dtos;
using AutoMapper;
using Domain.Dishes;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Restaurants;

namespace Application.Dishes;

public class DishesService : IDishesService
{
    private readonly IDishesRepository _dishRepository;
    private readonly IRestaurantsRepository _restaurantRepository;
    private readonly IMapper _mapper;
    public DishesService(IDishesRepository dishesRepository, IRestaurantsRepository restaurantsRepository, IMapper mapper )
    {
        _dishRepository = dishesRepository;
        _restaurantRepository = restaurantsRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<DishDto>> GetAllAsync(int restaurantId)
    {
        await ExistsRestaurantAsync(restaurantId);

        var dishes = await _dishRepository.GetAllDishesAsync(restaurantId);
        var dto = _mapper.Map<IEnumerable<DishDto>>(dishes);
        return dto;
    }

    public async Task<DishDto?> GetByIdAsync(int restaurantId, int id)
    {
        await ExistsRestaurantAsync(restaurantId);

        var dish = await _dishRepository.GetDishByIdAsync(restaurantId, id);
        if (dish == null)
        {
            throw new NotFoundException($"Dish with id {id} does not exist in restaurant with id {restaurantId}.");
        }
        var dto = _mapper.Map<DishDto?>(dish);
        return dto;
    }

    public async Task<IEnumerable<DishListDto>> GetListAsync(int restaurantId)
    {
        await ExistsRestaurantAsync(restaurantId);

        var list = await _dishRepository.GetListDishesAsync(restaurantId);
        var dto = _mapper.Map<IEnumerable<DishListDto>>(list);
        return dto;
    }
    public async Task<int> CreateAsync(int restaurantId, CreateDishDto dto)
    {
        await ExistsRestaurantAsync(restaurantId);

        var entity = _mapper.Map<Dish>(dto);
        entity.RestaurantId = restaurantId;
        await _dishRepository.CreateDishAsync(entity);
        return entity.Id;
    }
    public async Task UpdateAsync(int restaurantId, int id, UpdateDishDto dto)
    {
        await ExistsRestaurantAsync(restaurantId);

        var entity = await _dishRepository.GetDishByIdAsync(restaurantId, id);
        if (entity == null)
        {
            throw new NotFoundException($"Dish with id {id} does not exist in restaurant with id {restaurantId}.");
        }
        var updated = _mapper.Map(dto, entity);
        await _dishRepository.UpdateDishAsync(updated);
    }
    public async Task DeleteAsync(int restaurantId, int id)
    {
        await ExistsRestaurantAsync(restaurantId);

        var entity = await _dishRepository.GetDishByIdAsync(restaurantId, id);
        if (entity == null)
        {
            throw new NotFoundException($"Dish with id {id} does not exist in restaurant with id {restaurantId}.");
        }
        await _dishRepository.DeleteDishAsync(entity);
    }
    public async Task DeleteDishesForRestaurantAsync(int restaurantId)
    {
        await ExistsRestaurantAsync(restaurantId);

        var dishes = await _dishRepository.GetAllDishesAsync(restaurantId);
        if (!dishes.Any())
        {
            throw new NotFoundException($"No dishes found for restaurant with id {restaurantId}.");
        }
        await _dishRepository.DeleteDishesAsync(dishes);
    }
    private async Task ExistsRestaurantAsync(int restaurantId)
    {
        if (!await _restaurantRepository.ExistsAsync(restaurantId))
        {
            throw new NotFoundException($"Restaurant with id {restaurantId} does not exist.");
        }
    }
}
