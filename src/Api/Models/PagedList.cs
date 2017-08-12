using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Models
{
    public class PagedList<T> : List<T>, IWeakEntityTag
        where T : IWeakEntityTag
    {
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (items.Any())
            {
                ModifiedDate = items.OrderByDescending(x => x.ModifiedDate).First().ModifiedDate;
                AddRange(items);
            }
        }

        public int CurrentPage { get; }

        public int TotalPages { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public bool IsFirstPage => CurrentPage == 1;

        public bool IsLastPage => CurrentPage == TotalPages;

        public DateTime ModifiedDate { get; }
    }
}
