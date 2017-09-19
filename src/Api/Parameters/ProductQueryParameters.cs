namespace Api.Parameters
{
    public class ProductQueryParameters : PagingParameters
    {
        public string SearchQuery { get; set; }

        public string Color { get; set; }
    }
}
