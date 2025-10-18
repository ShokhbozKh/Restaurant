using System.ComponentModel;

namespace Domain.Commands;

public class GetAllRestaurantsQuery
{
    public string? Search {  get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; } 
}
