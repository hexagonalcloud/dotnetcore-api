namespace Core.Parameters
{
    /// <summary>
    /// The parameters
    /// </summary>
    public class ProductQueryParameters : PagingParameters
    {
        /// <summary>
        /// Search for values within the product name, for instance <b>bike stand</b> or <b>mountain</b>
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// One or more colors to filter on, for instance <b>black</b> or <b>black, blue</b>
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// By default the results are ordered by <b>name (ascending)</b> You can order by <b>color, color desc, color asc, name, name asc, name desc</b> or a combination.
        /// </summary>
        public string OrderBy { get; set; } = "Name";

        /// <summary>
        /// The fields you want to see in the results. For instance <b>id, name, color</b>
        /// </summary>
        public string Fields { get; set; }
    }
}
