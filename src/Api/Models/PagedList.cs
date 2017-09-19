using System;
using System.Collections.Generic;
using System.Linq;
using Api.Parameters;

namespace Api.Models
{
    public class PagedList<T> : List<T>, IWeakEntityTag, IPagedList
        where T : IWeakEntityTag
    {
        public PagedList(List<T> items, int count, PagingParameters pagingParameters)
        {
            PagingParameters = pagingParameters;

            TotalCount = count;
            PageSize = pagingParameters.PageSize;
            CurrentPage = pagingParameters.PageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pagingParameters.PageSize);
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

        public PagingParameters PagingParameters { get; }
    }
}
