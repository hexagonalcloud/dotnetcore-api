using Api.Models;

namespace Api.Services
{
    public interface IUrlService
    {
        string CreateLinkHeader(string routeName, IPagedList pagedList);
    }
}
