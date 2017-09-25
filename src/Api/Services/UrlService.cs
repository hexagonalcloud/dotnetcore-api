using System.Collections.Generic;
using Core.Entities;
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

        public string GetLinkHeader(string routeName, IPagedList pagedList)
        {
            // for now based on https://developer.github.com/v3/#pagination
            if (pagedList.TotalCount == 0)
            {
                return string.Empty;
            }

            var linkHeaders = new List<string>();
            dynamic parameters = pagedList.PagingParameters.GetParameters();

            if (pagedList.HasNext)
            {
                parameters.PageNumber = pagedList.CurrentPage + 1;
                var linkHeader = "<" + _urlHelper.Link(routeName, parameters).ToLowerInvariant() + ">; rel=next";
                linkHeaders.Add(linkHeader);
            }

            if (!pagedList.IsLastPage)
            {
                parameters.PageNumber = pagedList.TotalPages;
                var linkHeader = "<" + _urlHelper.Link(routeName, parameters).ToLowerInvariant() + ">; rel=last";
                linkHeaders.Add(linkHeader);
            }

            if (!pagedList.IsFirstPage)
            {
                parameters.PageNumber = 1;
                var linkHeader = "<" + _urlHelper.Link(routeName, parameters).ToLowerInvariant() + ">; rel=first";
                linkHeaders.Add(linkHeader);
            }

            if (pagedList.HasPrevious)
            {
                parameters.PageNumber = pagedList.CurrentPage - 1;
                var linkHeader = "<" + _urlHelper.Link(routeName, parameters).ToLowerInvariant() + ">; rel=prev";
                linkHeaders.Add(linkHeader);
            }

            return string.Join(", ", linkHeaders.ToArray());
        }
    }
}
