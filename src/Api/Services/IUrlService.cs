using Core.Entities;

namespace Api.Services
{
    public interface IUrlService
    {
        string GetLinkHeader(string routeName, IPagedList pagedList);
    }
}
