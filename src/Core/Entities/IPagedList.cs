using System;
using Core.Parameters;

namespace Core.Entities
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

        DateTime LastModified { get; }

        PagingParameters PagingParameters { get; }
    }

    public interface IPagedList <T> : IPagedList
        where T: IBaseEntity
    {
    }
}
