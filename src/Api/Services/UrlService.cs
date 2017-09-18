using System.Collections.Generic;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlHelper _urlHelper;

        public UrlService(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string CreateLinkHeader(string routeName, IPagedList pagedList)
        {
            // for now based on https://developer.github.com/v3/#pagination
            // TODO: do not add page size if the query param is null?
            var linkHeaders = new List<string>();

            if (pagedList.HasNext)
            {
                var linkHeader = "<" + _urlHelper.Link(routeName, new { pageNumber = pagedList.CurrentPage + 1, pageSize = pagedList.PageSize, color = pagedList.FilterParameters.Color }).ToLowerInvariant() + ">; rel=next";
                linkHeaders.Add(linkHeader);
            }

            if (!pagedList.IsLastPage)
            {
                var linkHeader = "<" + _urlHelper.Link(routeName, new { pageNumber = pagedList.TotalPages, pageSize = pagedList.PageSize, color = pagedList.FilterParameters.Color }).ToLowerInvariant() + ">; rel=last";
                linkHeaders.Add(linkHeader);
            }

            if (!pagedList.IsFirstPage)
            {
                var linkHeader = "<" + _urlHelper.Link(routeName, new { pageNumber = 1, pageSize = pagedList.PageSize, color = pagedList.FilterParameters.Color }).ToLowerInvariant() + ">; rel=first";
                linkHeaders.Add(linkHeader);
            }

            if (pagedList.HasPrevious)
            {
                var linkHeader = "<" + _urlHelper.Link(routeName, new { pageNumber = pagedList.CurrentPage - 1, pageSize = pagedList.PageSize, color = pagedList.FilterParameters.Color }).ToLowerInvariant() + ">; rel=prev";
                linkHeaders.Add(linkHeader);
            }

            return string.Join(", ", linkHeaders.ToArray());
        }
    }
}
