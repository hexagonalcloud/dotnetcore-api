namespace Core.Parameters
{
    public class ProductQueryParameters : PagingParameters
    {
        public string SearchQuery { get; set; }

        public string Color { get; set; }

        public string OrderBy { get; set; } = "Name";
    }
}
