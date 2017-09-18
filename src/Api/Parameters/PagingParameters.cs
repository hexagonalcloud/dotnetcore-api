namespace Api.Parameters
{
    public class PagingParameters
    {
        protected const int MaxPageSize = 20;
        protected const int DefaultPageSize = 10;
        private int _pageNumber;
        private int _pageSize;

        public PagingParameters()
        {
            PageNumber = 1;
            PageSize = DefaultPageSize;
        }

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value > 0 ? value : 1;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= MaxPageSize && value > 0 ? value : DefaultPageSize;
        }
    }
}
