using Application.Restaurants.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Restaurants;
using Microsoft.Extensions.Logging;

namespace Application.Restaurants;

public class RestaurantsService: IRestaurantsService
{
    private readonly IRestaurantsRepository _repository;
    private readonly ILogger<RestaurantsService> _logger;
    private readonly IMapper mapper;
    public RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger, IMapper mapper )
    {
        _repository = restaurantsRepository;
        _logger = logger;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
    {
        _logger.LogInformation("Getting all restaurants");
        var restaurants = await _repository.GetAllAsync();
        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return restaurantDtos;
    }

    public async Task<RestaurantDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting restaurant by id: {id}", id);
        var restaurant = await _repository.GetByIdAsync(id);
        if(restaurant == null)
        {
            _logger.LogWarning("Restaurant not found with id: {id}", id);
            return null;
        }
        var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);
        return restaurantDto;
    }

    public async Task<IEnumerable<RestaurantListDto>> GetListAsync()
    {
        var list = await _repository.GetListAsync();
        var listDtos = mapper.Map<IEnumerable<RestaurantListDto>>(list); 
        return listDtos;
    }
    public async Task<int> CreateAsync(CreateRestaurantDto dto)
    {
        var entity = mapper.Map<Restaurant>(dto);
        await _repository.CreateAsync(entity);
        return entity.Id;
    }
    public async Task UpdateAsync(int id, UpdateRestaurantDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if(entity == null)
        {
            _logger.LogWarning("Restaurant not found with id: {id}", id);
            throw new KeyNotFoundException($"Restaurant not found with id {id}");
        }
        var updatedEntity = mapper.Map(dto, entity);
        await _repository.UpdateAsync(updatedEntity);
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if(entity == null)
        {
            _logger.LogWarning("Restaurant not found with id: {id}", id);
            throw new KeyNotFoundException($"Restaurant not found with id {id}");
        }
        await _repository.DeleteAsync(entity);
    }
}
