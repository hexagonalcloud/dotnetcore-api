using System;
using System.Linq;

namespace Api.Parameters
{
    public class ProductQueryParameters : PagingParameters
    {
        private readonly string[] _allowedOrdering = { "ProductID", "ProductID desc", "ProductID asc", "Color", "Color desc", "Color asc", "Name", "Name asc", "Name desc" };
        private string _orderBy = "ProductID";

        public string SearchQuery { get; set; }

        public string Color { get; set; }

        public string OrderBy
        {
            get => _orderBy;

            set
            {
                _orderBy = _allowedOrdering.Any(s => s.Equals(value, StringComparison.OrdinalIgnoreCase)) ? value : "ProductID";
            }
        }
    }
}
