﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; init; }
        public int PageNumber { get; init; }
        public int TotalPages { get; init; }
        public int TotalCount { get; init; }

        public PaginatedList()
        {

        }

        public PaginatedList(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            TotalCount = totalCount;
            Items = items;
        }        

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        
        
    }
}
