using Application.Common;
using Domain.Commands;
using Domain.Entities;
using Domain.Restaurants;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        var baseQuery = await _context.Restaurants
            .Include(d => d.Dishes)
            .ToListAsync();

        return baseQuery;
    }
    public async Task<(IEnumerable<Restaurant>, int)> GetPagedAsync(GetAllRestaurantsQuery query)
    {
        var baseQuery = _context.Restaurants
            .Include(r => r.Dishes)
            .AsQueryable();

        // Search
        var search = query.Search?.ToLower().Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            baseQuery = baseQuery.Where(r =>
                r.Name.ToLower().Contains(search) ||
                r.Description.ToLower().Contains(search));
        }

        // Total count (before paging)
        var totalCount = await baseQuery.CountAsync();

        // Sorting
        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var columnSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(Restaurant.Name), r => r.Name },
            { nameof(Restaurant.Category), r => r.Category },
            { nameof(Restaurant.Description), r => r.Description }
        };

            if (columnSelectors.TryGetValue(query.SortBy, out var selectedColumn))
            {
                baseQuery = query.SortDirection == SortDirection.Descending
                    ? baseQuery.OrderByDescending(selectedColumn)
                    : baseQuery.OrderBy(selectedColumn);
            }
        }
        else
        {
            // Default sort (agar SortBy berilmagan bo‘lsa)
            baseQuery = baseQuery.OrderBy(r => r.Id);
        }

        // Paging
        var items = await baseQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (items, totalCount); // return items along with total count
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
    public async Task<bool> ExistsAsync(int restaurantId)
    {
        return await _context.Restaurants.AnyAsync(r => r.Id == restaurantId);
    }
}
