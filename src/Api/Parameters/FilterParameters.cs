using System.Collections.Generic;

namespace Api.Parameters
{
    public class FilterParameters
    {
        public string Color { get; set; }

        // so basically we want to grab any properites on this that have values and put them in dictionary key value
        // then spit out that dictionary to pass into the PagedList and build up our header url?
        // the url helper will ignore params that do not have values it seems....

        public IDictionary<string,string> GetFilters()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Color", "Black");
            return dict;
        }
    }
}
