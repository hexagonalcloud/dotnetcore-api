using Core.Entities;

namespace RestApi.Services
{
    public interface IUrlService
    {
        string GetLinkHeader(string routeName, IPagedList pagedList);
    }
}
