using Application.Dishes;
using Application.Dishes.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Restaurants.Api.Controllers;
[ApiController]
[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController: ControllerBase
{
    private readonly IDishesService _service;

    public DishesController(IDishesService dishesService)
    {
        _service = dishesService;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDish([FromRoute]int restaurantId)
    {
        var dishes = await  _service.GetAllAsync(restaurantId);
        return Ok(dishes);
    }
    [HttpGet("{dishId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDishById([FromRoute]int restaurantId, [FromRoute]int dishId)
    {
        var dish = await _service.GetByIdAsync(restaurantId, dishId);
        return Ok(dish);
    }
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDishList([FromRoute]int restaurantId)
    {
        var list = await _service.GetListAsync(restaurantId);
        return Ok(list);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDish([FromRoute]int restaurantId, [FromBody] CreateDishDto dto)
    {
        var dishId = await  _service.CreateAsync(restaurantId, dto);
        return CreatedAtAction(nameof(GetDishById), new { restaurantId = restaurantId, dishId = dishId }, null);
    }
    [HttpPut("{dishId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDish([FromRoute]int restaurantId, [FromRoute]int dishId, [FromBody] UpdateDishDto dto)
    {
        await _service.UpdateAsync(restaurantId, dishId, dto);
        return NoContent();
    }
    [HttpDelete("{dishId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDish([FromRoute]int restaurantId, [FromRoute]int dishId)
    {
        await _service.DeleteAsync(restaurantId, dishId);
        return NoContent();
    }
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAllDishes([FromRoute]int restaurantId)
    {
        await _service.DeleteDishesForRestaurantAsync(restaurantId);
        return NoContent();
    }
}
