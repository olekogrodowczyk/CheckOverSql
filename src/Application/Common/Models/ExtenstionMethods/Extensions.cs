using AutoMapper;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.ExtenstionMethods
{
    public static class Extensions
    {
        public static async Task<PaginatedList<T>> PaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }     
        
        public static async Task<PaginatedList<TResult>> MapPaginatedList<TResult,TSource>(this PaginatedList<TSource> source, IMapper mapper)
        {
            var items = mapper.Map<List<TSource>,List<TResult>>(source.Items);
            return await Task.FromResult (new PaginatedList<TResult>
            {
                Items = items,
                TotalCount = source.TotalCount,
                TotalPages = source.TotalPages,
                PageNumber = source.PageNumber,
            });
        }
    }
}
