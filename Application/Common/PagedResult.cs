using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common;

public class PagedResult<T> where T : class
{
    public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        PageNumber = pageNumber;

        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        ItemsFrom = (pageSize * (pageNumber - 1)) + 1;
        ItemsTo = Math.Min(ItemsFrom + items.Count() - 1, totalCount);
    }

    public IEnumerable<T> Items { get; set; }

    public int TotalCount { get; set; }     // umumiy elementlar soni
    public int PageSize { get; set; }       // sahifadagi elementlar soni
    public int PageNumber { get; set; }     // joriy sahifa
    public int TotalPages { get; set; }     // jami sahifalar soni

    public int ItemsFrom { get; set; }      // nechinchi elementdan boshlangan
    public int ItemsTo { get; set; }        // nechinchi elementgacha

    //public bool HasPreviousPage => PageNumber > 1;
    //public bool HasNextPage => PageNumber < TotalPages;
}
