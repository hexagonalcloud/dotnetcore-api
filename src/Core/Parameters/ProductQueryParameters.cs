using System;
using System.Linq;

namespace Core.Parameters
{
    public class ProductQueryParameters : PagingParameters
    {
        private readonly string[] _allowedOrdering = { "Color", "Color desc", "Color asc", "Name", "Name asc", "Name desc" };

        public string SearchQuery { get; set; }

        public string Color { get; set; }

        public string OrderBy { get; set; } = "Name";
    }
}
