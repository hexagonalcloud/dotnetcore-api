using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Core.Parameters;

namespace Core.Entities
{
    public class PagedList<T> : List<T>, IPagedList<T>
        where T : IBaseEntity
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
                LastModified = items.OrderByDescending(x => x.ModifiedDate).First().ModifiedDate;
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

        public DateTime LastModified { get; }

        public PagingParameters PagingParameters { get; }

        public IEnumerable<ExpandoObject> SelectFields(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                throw new ArgumentNullException(nameof(fields));
            }

            var result = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();
            var fieldsToReturn = fields.Split(',');

            foreach (var field in fieldsToReturn)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(T)
                    .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    propertyInfoList.Add(propertyInfo);
                }
            }

            if (propertyInfoList.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fields));
            }

            foreach (T source in this)
            {
                var expando = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expando).Add(propertyInfo.Name, propertyValue);
                }

                result.Add(expando);
            }

            return result;
        }
    }
}
