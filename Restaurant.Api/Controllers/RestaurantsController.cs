using Application.Restaurants;
using Application.Restaurants.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Restaurants.Api.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController:ControllerBase
{
    private readonly IRestaurantsService _service;
    public RestaurantsController(IRestaurantsService restaurantsService)
    {
        _service = restaurantsService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await _service.GetAllAsync();
        return Ok(restaurants);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var restaurant = await _service.GetByIdAsync(id);
        if(restaurant == null)
        {
            return NotFound($"Restaurant not found with id {id}");
        }
        return Ok(restaurant);
    }
    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        var list = await _service.GetListAsync();
        return Ok(list);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDto dto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var entityId = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = entityId }, null);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRestaurantDto dto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
