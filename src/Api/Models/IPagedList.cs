using System;
using Api.Parameters;

namespace Api.Models
{
    public interface IPagedList
    {
        int CurrentPage { get; }

        int TotalPages { get; }

        int PageSize { get; }

        int TotalCount { get; }

        bool HasPrevious { get; }

        bool HasNext { get; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }

        DateTime ModifiedDate { get; }

        FilterParameters FilterParameters { get; }

        PagingParameters PagingParameters { get; }
    }
}
